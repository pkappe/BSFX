//+------------------------------------------------------------------+
//|                                                      ProjectName |
//|                                      Copyright 2012, CompanyName |
//|                                       http://www.companyname.net |
//+------------------------------------------------------------------+
input double Move=0.01;
input double StopLoss=1;
input double Goal=0.003;
input double lotSize=1;
int ticket;
double spread;
bool justTraded;
double pairMove;
string Side;
double PastPrice;
double PastPriceCheck;
double double_array[][6];
//+------------------------------------------------------------------+
//|   INITIALIZE                                                     |
//+------------------------------------------------------------------+

int OnInit()
  {
   justTraded=False;
   if(ObjectGet("Label",OBJPROP_NAME))
     {
      Print("MOVE LABEL ALREADY EXISTS");
     }
   else
     {
     LabelCreate(0,"Label",0,0,0,3,"","Consolas",20,clrAntiqueWhite,0.000000,4,false,false,false,0);      
     }
   return(INIT_SUCCEEDED);
  }
//+------------------------------------------------------------------+
//|   ON TICK EVENT                                                  |
//+------------------------------------------------------------------+

void OnTick()
  {
// Grab the open price from the current candle
   ArrayCopyRates(double_array,NULL,0);
   PastPriceCheck=double_array[0][1];
// Find if the open price is a new candle.
   if(PastPriceCheck==PastPrice)
     {
      PastPrice=PastPrice;
     }
   else
     {
      // Let the pair trade again if it is a new candle.
      PastPrice=PastPriceCheck;
      justTraded=false;
     }
// Calculate Spread
   spread=Ask-Bid;

// Calculate the move
   pairMove=Bid-PastPrice;
   string textMove=(string)(pairMove*10000);
   int index=StringFind(textMove,".",0);
   string textStart=StringSubstr(textMove,0,index);
   string textEnd=StringSubstr(textMove,index,2);
   string textFinal=textStart+textEnd;
   ObjectSetString(0,"Label",OBJPROP_TEXT,textFinal);
// See if the symbol is correct
   if(justTraded==false)
     {

      // LONG POSITION ENTRY CHECK
      if(pairMove>=Move)
        {
         double stopLoss=NULL;
         Print(stopLoss);
         double goal=Goal+Ask;
         justTraded = true;
         ticket=OrderSend(Symbol(),OP_BUY,lotSize,Ask,3,stopLoss,goal,NULL,0,0,clrGreen);
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
      if(pairMove<=(Move*-1))
        {
         justTraded=true;
         double shortStop=NULL;
         double shortGoal=Bid-Goal;
         ticket=OrderSend(Symbol(),OP_SELL,lotSize,Bid,3,shortStop,shortGoal,NULL,0,0,clrGreen);
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

//+------------------------------------------------------------------+
//| Create a text label                                              |
//+------------------------------------------------------------------+
bool LabelCreate(const long              chart_ID=0,               // chart's ID
                 const string            name="Label",             // label name
                 const int               sub_window=0,             // subwindow index
                 const int               x=0,                      // X coordinate
                 const int               y=0,                      // Y coordinate
                 const ENUM_BASE_CORNER  corner=CORNER_RIGHT_LOWER,// chart corner for anchoring
                 const string            text="",// text
                 const string            font="Arial",             // font
                 const int               font_size=10,             // font size
                 const color             clr=clrAntiqueWhite,      // color
                 const double            angle=0.0,                // text slope
                 const ENUM_ANCHOR_POINT anchor=ANCHOR_LEFT_UPPER, // anchor type
                 const bool              back=false,               // in the background
                 const bool              selection=false,          // highlight to move
                 const bool              hidden=false,// hidden in the object list
                 const long              z_order=0) // priority for mouse click
  {
//--- reset the error value
   ResetLastError();
//--- create a text label
   if(!ObjectCreate(chart_ID,name,OBJ_LABEL,sub_window,0,0))
     {
      Print(__FUNCTION__,
            ": failed to create text label! Error code = ",GetLastError());
      return(false);
     }
//--- set label coordinates
   ObjectSetInteger(chart_ID,name,OBJPROP_XDISTANCE,x);
   ObjectSetInteger(chart_ID,name,OBJPROP_YDISTANCE,y);
//--- set the chart's corner, relative to which point coordinates are defined
   ObjectSetInteger(chart_ID,name,OBJPROP_CORNER,corner);
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
//--- enable (true) or disable (false) the mode of moving the label by mouse
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
//| Move the text label                                              |
//+------------------------------------------------------------------+
bool LabelMove(const long   chart_ID=0,   // chart's ID
               const string name="Label", // label name
               const int    x=0,          // X coordinate
               const int    y=0)          // Y coordinate
  {
//--- reset the error value
   ResetLastError();
//--- move the text label
   if(!ObjectSetInteger(chart_ID,name,OBJPROP_XDISTANCE,x))
     {
      Print(__FUNCTION__,
            ": failed to move X coordinate of the label! Error code = ",GetLastError());
      return(false);
     }
   if(!ObjectSetInteger(chart_ID,name,OBJPROP_YDISTANCE,y))
     {
      Print(__FUNCTION__,
            ": failed to move Y coordinate of the label! Error code = ",GetLastError());
      return(false);
     }
//--- successful execution
   return(true);
  }
//+------------------------------------------------------------------+
//| Change the object text                                           |
//+------------------------------------------------------------------+
bool LabelTextChange(const long   chart_ID=0,   // chart's ID
                     const string name="Label", // object name
                     const string text="Text")  // text
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
//| The function moves the object and changes its text               |
//+------------------------------------------------------------------+
bool MoveAndTextChange(const int x,const int y,string text)
  {
//--- move the label
   if(!LabelMove(0,"Label",x,y))
      return(false);
//--- change the label text
   text=StringConcatenate(text,x,",",y);
   if(!LabelTextChange(0,"Label",text))
      return(false);
//--- check if the script's operation has been forcefully disabled
   if(IsStopped())
      return(false);
//--- redraw the chart
   ChartRedraw();
// 0.01 seconds of delay
   Sleep(10);
//--- exit the function
   return(true);
  }
//+------------------------------------------------------------------+
