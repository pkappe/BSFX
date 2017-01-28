//+------------------------------------------------------------------+
//|                            GBP/JPY                               |
//|                 This EA is specifically designed                 |
//|             for the Great Britain Pound/Japanese Yen             |
//+------------------------------------------------------------------+

input double Move=0.0250;
input double StopLoss=0.0100;
input double Goal=0.0100;
input double MaxSpread=0.0005;
input double lotSize=0.100;

int ticket;
double spread;
bool justTraded;
double gbpjpyBid;
double gbpjpyAsk;
double gbpjpyMove;
string gbpjpySide;
double gbpjpyPastPrice;
double double_array[][6];
//+------------------------------------------------------------------+
//|   INITIALIZE                                                     |
//+------------------------------------------------------------------+

int OnInit()
  {
   justTraded=False;   
   TextCreate(0,"MoveText",0,0,0.000000,NULL,"Consolas",24,clrAntiqueWhite,0.000000,0,false,false,true,0);
   return(INIT_SUCCEEDED);
  }
//+------------------------------------------------------------------+
//|   ON TICK EVENT                                                  |
//+------------------------------------------------------------------+

void OnTick()
  {
// See if the symbol is correct
   if(Symbol()=="GBPJPY" && justTraded==false)
     {
      // Set Bid/Ask and Spread
      gbpjpyBid = MarketInfo("GBPJPY",MODE_BID)/100;
      gbpjpyAsk = MarketInfo("GBPJPY",MODE_ASK)/100;
      spread=gbpjpyAsk-gbpjpyBid;
      if(spread<0)
        {
         spread=spread*-1;
        }
      // If Past Price hasn't been set, set it
      if(gbpjpyPastPrice==0)
        {
         // Grab the open price from the current candle
         ArrayCopyRates(double_array,NULL,0);
         gbpjpyPastPrice=double_array[0][1]/100;
        }
      // Calculate the move
      gbpjpyMove=gbpjpyAsk-gbpjpyPastPrice;
      string textMove=gbpjpyMove*10000;
      ObjectSetString(0,"MoveText",OBJPROP_TEXT,textMove);

      // LONG POSITION ENTRY CHECK
      if(gbpjpyMove>=Move && spread<=MaxSpread && justTraded==false)
        {
         double stopLoss=gbpjpyBid-StopLoss;
         double goal=Goal+gbpjpyBid;
         ticket=OrderSend("GBPJPY",OP_BUY,lotSize,gbpjpyBid,3,stopLoss,goal,NULL,0,0,clrGreen);
         if(ticket<0)
           {
            Print("OrderSend failed with error #",GetLastError());
           }
         else
           {
            justTraded=True;
            Print("OrderSend placed successfully");
           }
        }
      // SHORT POSITION ENTRY CHECK
      if(gbpjpyMove<=(Move*-1) && spread<MaxSpread && justTraded==false)
        {
         double shortStop=gbpjpyAsk+StopLoss;
         double shortGoal=Goal-gbpjpyAsk;
         ticket=OrderSend("GBPJPY",OP_SELL,lotSize,gbpjpyAsk,3,shortStop,shortGoal,NULL,0,0,clrGreen);
         if(ticket<0)
           {
            Print("OrderSend failed with error #",GetLastError());
           }
         else
           {
            justTraded=True;
            Print("OrderSend placed successfully");
           }
        }
     }
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+

void OnDeinit(const int reason)
  {
   
  }
//+------------------------------------------------------------------+

bool TextCreate(const long              chart_ID=0,               // chart's ID
                const string            name="MoveText",          // object name
                const int               sub_window=0,             // subwindow index
                datetime                time=0,                   // anchor point time
                double                  price=0,                  // anchor point price
                const string            text="",                  // the text itself
                const string            font="Consolas",// font
                const int               font_size=24,// font size
                const color             clr=clrAntiqueWhite,// color
                const double            angle=0.0,                // text slope
                const ENUM_ANCHOR_POINT anchor=ANCHOR_LEFT_UPPER, // anchor type
                const bool              back=false,               // in the background
                const bool              selection=false,          // highlight to move
                const bool              hidden=true,              // hidden in the object list
                const long              z_order=0)                // priority for mouse click
  {
//--- set anchor point coordinates if they are not set
   ChangeTextEmptyPoint(time,price);
//--- reset the error value
   ResetLastError();
//--- create Text object
   if(!ObjectCreate(chart_ID,name,OBJ_TEXT,sub_window,time,price))
     {
      Print(__FUNCTION__,
            ": failed to create \"Text\" object! Error code = ",GetLastError());
      return(false);
     }
//--- set the text
   ObjectSetString(chart_ID,name,OBJPROP_TEXT,text);
//--- set text font
   ObjectSetString(chart_ID,name,OBJPROP_FONT,font);
//--- set font size
   ObjectSetInteger(chart_ID,name,OBJPROP_FONTSIZE,font_size);
//--- set the slope angle of the text
   ObjectSetDouble(chart_ID,name,OBJPROP_ANGLE,angle);
//--- set anchor type
   ObjectSetInteger(chart_ID,name,OBJPROP_ANCHOR,anchor);
//--- set color
   ObjectSetInteger(chart_ID,name,OBJPROP_COLOR,clr);
//--- display in the foreground (false) or background (true)
   ObjectSetInteger(chart_ID,name,OBJPROP_BACK,back);
//--- enable (true) or disable (false) the mode of moving the object by mouse
   ObjectSetInteger(chart_ID,name,OBJPROP_SELECTABLE,selection);
   ObjectSetInteger(chart_ID,name,OBJPROP_SELECTED,selection);
//--- hide (true) or display (false) graphical object name in the object list
   ObjectSetInteger(chart_ID,name,OBJPROP_HIDDEN,hidden);
//--- set the priority for receiving the event of a mouse click in the chart
   ObjectSetInteger(chart_ID,name,OBJPROP_ZORDER,z_order);
//--- successful execution
   return(true);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
bool TextChange(const long   chart_ID=0,// chart's ID
                const string name="Text", // object name
                const string text="Text") // text
  {
//--- reset the error value
   ResetLastError();
//--- change object text
   if(!ObjectSetString(chart_ID,name,OBJPROP_TEXT,text))
     {
      Print(__FUNCTION__,
            ": failed to change the text! Error code = ",GetLastError());
      return(false);
     }
//--- successful execution
   return(true);
  }
//+------------------------------------------------------------------+
//|                                                                  |
//+------------------------------------------------------------------+
void ChangeTextEmptyPoint(datetime &time,double &price)
  {
//--- if the point's time is not set, it will be on the current bar
   if(!time)
      time=TimeCurrent();
//--- if the point's price is not set, it will have Bid value
   if(!price)
      price=SymbolInfoDouble(Symbol(),SYMBOL_BID);
  }
//+------------------------------------------------------------------+
