using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fxcore2;

namespace BSFX
{
	internal class OrderMonitor
	{
		static public bool IsOpeningOrder(O2GOrderRow order)
		{
			return order.Type.StartsWith("O");
		}


		private enum OrderState
		{
			OrderExecuting,
			OrderExecuted,
			OrderCanceled,
			OrderRejected
		}
		private volatile OrderState mState;

		private O2GTradeRow mTrade;
		private volatile int mTotalAmount;
		private volatile int mRejectAmount;
		private O2GOrderRow mOrder;
		private string mRejectMessage;

		public enum ExecutionResult
		{
			Executing,
			Executed,
			PartialRejected,
			FullyRejected,
			Canceled
		};

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="order">Order for monitoring of execution</param>
		public OrderMonitor(O2GOrderRow order)
		{
			mOrder = order;
			mRejectAmount = 0;
			mState = OrderState.OrderExecuting;
			mResult = ExecutionResult.Executing;
			mTrade = null;
		}

		/// <summary>
		/// Process trade adding during order execution
		/// </summary>
		public void OnTradeAdded(O2GTradeRow tradeRow)
		{
			String tradeOrderID = tradeRow.OpenOrderID;
			String orderID = mOrder.OrderID;
			if (tradeOrderID == orderID)
			{
				mTrade = tradeRow;

				if (mState == OrderState.OrderExecuted ||
					mState == OrderState.OrderRejected ||
					mState == OrderState.OrderCanceled)
				{
					if (IsAllTradeReceived())
						SetResult(true);
				}
			}
		}

		/// <summary>
		/// Process order data changing during execution
		/// </summary>
		public void OnOrderChanged(O2GOrderRow orderRow)
		{
			//STUB
		}

		/// <summary>
		/// Process order deletion as result of execution
		/// </summary>
		public void OnOrderDeleted(O2GOrderRow orderRow)
		{
			String deletedOrderID = orderRow.OrderID;
			String orderID = mOrder.OrderID;


			if (deletedOrderID == orderID)
			{
				// Store Reject amount
				if (OrderRowStatus.Rejected.Equals(orderRow.Status))
				{
					mState = OrderState.OrderRejected;
					mRejectAmount = orderRow.Amount;
					mTotalAmount = orderRow.OriginAmount - mRejectAmount;

					if (!string.IsNullOrEmpty(mRejectMessage) && IsAllTradeReceived())
						SetResult(true);
				}
				else if (OrderRowStatus.Canceled.Equals(orderRow.Status))
				{
					mState = OrderState.OrderCanceled;
					mRejectAmount = orderRow.Amount;
					mTotalAmount = orderRow.OriginAmount - mRejectAmount;
					if (IsAllTradeReceived())
						SetResult(false);
				}
				else
				{
					mRejectAmount = 0;
					mTotalAmount = orderRow.OriginAmount;
					mState = OrderState.OrderExecuted;
					if (IsAllTradeReceived())
						SetResult(true);
				}
			}
		}

		/// <summary>
		/// Process reject message as result of order execution
		/// </summary>
		public void OnMessageAdded(O2GMessageRow messageRow)
		{
			if (mState == OrderState.OrderRejected ||
				mState == OrderState.OrderExecuting)
			{
				bool IsRejectMessage = CheckAndStoreMessage(messageRow);
				if (mState == OrderState.OrderRejected && IsRejectMessage)
					SetResult(true);
			}
		}

		/// <summary>
		/// Event about order execution is completed and all affected trades as opened/closed, all reject/cancel processed
		/// </summary>
		public event EventHandler OrderCompleted;

		/// <summary>
		/// Result of Order execution
		/// </summary>
		public ExecutionResult Result
		{
			get
			{
				return mResult;
			}
		}
		private volatile ExecutionResult mResult;

		/// <summary>
		/// Order execution is completed (with any result)
		/// </summary>
		public bool IsOrderCompleted
		{
			get
			{
				return (mResult != ExecutionResult.Executing);
			}
		}

		/// <summary>
		/// Monitored order
		/// </summary>
		public O2GOrderRow Order
		{
			get
			{
				return mOrder;
			}
		}

		/// <summary>
		/// List of Closed Trades which were opened as effects of order execution
		/// </summary>
		public O2GTradeRow Trade
		{
			get
			{
				return mTrade;
			}
		}


		/// <summary>
		/// Amount of rejected part of order
		/// </summary>
		public int RejectAmount
		{
			get
			{
				return mRejectAmount;
			}
		}

		/// <summary>
		/// Info message with a reason of reject
		/// </summary>
		public string RejectMessage
		{
			get
			{
				return mRejectMessage;
			}
		}


		private void SetResult(bool success)
		{
			if (success)
			{
				if (mRejectAmount == 0)
					mResult = ExecutionResult.Executed;
				else
					mResult = (mTrade == null) ? ExecutionResult.FullyRejected : ExecutionResult.PartialRejected;
			}
			else
				mResult = ExecutionResult.Canceled;

			if (OrderCompleted != null)
				OrderCompleted(this, EventArgs.Empty);

		}

		private bool IsAllTradeReceived()
		{
			if (mState == OrderState.OrderExecuting)
				return false;
			int currenTotalAmount = 0;
			if (mTrade != null)
			{
				currenTotalAmount += mTrade.Amount;
			}

			return currenTotalAmount == mTotalAmount;
		}

		private bool CheckAndStoreMessage(O2GMessageRow message)
		{
			String feature;
			feature = message.Feature;
			if (MessageFeature.MarketCondition.Equals(feature))
			{
				String text = message.Text;
				int findPos = text.IndexOf(mOrder.OrderID);
				if (findPos > -1)
				{
					mRejectMessage = text;
					return true;
				}
			}
			return false;
		}

	}

	internal class OrderRowStatus
	{
		public static string Rejected = "R";
		public static string Canceled = "C";
		public static string Executed = "F";
		//...
	}

	internal class MessageFeature
	{
		public static String MarketCondition = "5";
		//...
	}
}
