// bignumber.cs
// functions for handling really large numbers
//

function BigNumberFromString(%string)
   {
    %num = new ScriptObject(BigNumber);
    %aString = expandNumber(%string);
    if ( getSubStr(%aString, 0, 1) $= "-")
      {
       %num.sign= -1;
       %aString = getSubStr(%aString, 1, strlen(%aString) -1);
      }
    else
       %num.sign=1;

    %len= strlen(%aString);
    %partNum = 1;
    %part="";
    for ( %a=%len; %a > -1; %a--)
      {
       %part = getSubStr(%aString, %a, 1) @ %part;
       if ( strlen(%part) == 5)
         {
          %num.p[%partNum] = %part;
          %partNum++;
          %part="";
         }
      }
    if ( %part > 0)
      {
       %num.p[%partNum] = %part;
       %num.maxPart=%partNum;
      }
    else
      %num.maxPart = %partNum-1;
    %num.changed=true;
    return %num;
   }

// set number to 0
function BigNumber::zero(%this)
   {
    %this.maxpart=1;
    %this.p1 = "0";
    %this.p2 = "";
    %this.p3 = "";
    %this.p4 = "";
    %this.sign=1;
    %this.display="0";
   }

function BigNumber::Add(%this, %aNumber)
   {
    if ( %aNumber == 0)
      {
       return %this;
      }
    %aNumber = expandNumber(%aNumber);
    if ( %this.sign ==1)
      {
       if ( getSubStr(%aNumber, 0, 1) $= "-")
         {
          return %this.DoSubtract( getSubStr(%aNumber, 1, strlen(%aNumber) ) );
         }
       return %this.doAdd(%aNumber);
      }

    if ( %this.sign == -1)
      {
       if ( getSubStr(%aNumber, 0, 1) $= "-")
         {
          return %this.doAdd( getSubStr(%aNumber, 1, strlen(%aNumber) ) );
         }
       return %this.DoSubtract( %aNumber );
      }

    error("WTF happened? " @ %aNumber);
    %this.dump();
   }

function BigNumber::Subtract(%this, %aNumber)
   {
    if ( %aNumber == 0)
      {
       return %this;
      }
    %aNumber = expandNumber(%aNumber);
    if ( %this.sign == 1)
      {
       //echo("subtract: sign +");
       if ( getSubstr(%aNumber, 0, 1) $= "-")
         {
          //echo("subtract: aNumber < 0");
          return %this.DoAdd( getSubStr(%aNumber, 1, strlen(%aNumber) ) );
         }
       //echo("subtract: aNumber > 0");
       return %this.DoSubtract(%aNumber);
      }

    if ( %this.sign == -1)
      {
       //echo("subtract: sign -");
       if ( getSubStr(%aNumber, 0, 1) $= "-")
         {
          return %this.DoSubTract( getSubStr(%aNumber, 1, strlen(%aNumber) ));
         }
       return %this.DoAdd( mabs(%aNumber) );
      }

    error("WTF happened? " @ %aNumber);
    %this.dump();
   }

function BigNumber::DoAdd(%this, %aNumber)
   {
    if ( strpos(%aNumber, ".") > 0)
      {
       error("decimal in number " @ %aNumber);
      }

    %num = %aNumber;
    %partNum = 1;

    while ( %num > 0)
     {
      %len = strlen(%num);
      if ( %len < 6 )
        {
         %this.p[%partNum] += %num;
         %num = 0;
        }
      else
        {
         %add = getSubStr(%num, %len-5, %len);
         //echo("add: " @ %add);
         %this.p[%partNum] += %add;
         %num = getSubStr(%num, 0, %len-5);
         //echo("num: " @ %num);
        }

      if ( %this.p[%partNum] > 99999 )
        {
         %len = strlen(%this.p[%partNum] );
         %add = getSubStr(%this.p[%partNum], 0, %len-5 );
         //echo("add p1: " @ %add);
         %this.p[%partNum+1] += %add;
         %this.p[%partNum] = getSubStr(%this.p[%partNum], %len-5, %len);
        }
      %partNum++;
     }
    if ( %partNum > %this.maxPart)
      {
       %this.maxPart = %partNum;
      }
    //%this.dump();
    %this.changed=true;
    return %this;
   }

function BigNumber::DoSubtract(%this, %aNumber)
   {
    if ( strpos(%aNumber, ".") > 0)
      {
       error("decimal in number " @ %aNumber);
      }
    if ( %this.LessThanABS(%aNumber) )
      {
       //echo("subtract " @ %this.display() @ " - " @ %aNumber);
       //echo("do reverse subtract " @ %aNumber @ " - " @ %this.display() );

       %tmp = bigNumberFromString(%aNumber);
       %thisSign = %this.sign;
       %tmpSign = %tmp.sign;
       %this.sign = 1; %this.changed=true;
       %tmp.sign = 1;
       %tmp = %tmp.subtract(%this.display() );

       // now copy all the values from %tmp over to %this
       %this.p1 = %tmp.p1;
       %this.p2 = %tmp.p2;
       %this.p3 = %tmp.p3;

       // set the sign
       if ( %thisSign == %tmpSign )
         {
          %this.sign = %tmpSign * -1;
         }
       else
         {
          %this.sign = %tmpSign;
         }
       %this.MaxPart = %tmp.maxPart;
       if ( %this.maxPart > 3)
         {
          for ( %a=1; %a <= %this.maxPart; %a++)
            {
             %this.p[%a] = %tmp.p[%a];
            }
         }
       %this.changed = true;
       return %this;
      }

    //echo("Subtract " @ %aNumber);
    %num = %aNumber;
    %partNum = 1;
    %sign = %this.sign;

    while ( %num > 0)
      {
       //echo("part: "@ %partNum @ " num: " @ %num);
       %len = strlen(%num);
       if ( %len < 6 )
         {
          %this.p[%partNum] -= %num;
          %num = 0;
         }
       else
         {
          %sub = getSubStr(%num, %len-5, %len);
          //echo("sub: " @ %sub);
          %this.p[%partNum] -= %sub;
          //echo("%this.p" @ %partNum @ " = " @ %this.p[%partNum]);
          %num = getSubStr(%num, 0, %len-5);
          //echo("num: " @ %num);
         }

       if ( %this.p[%partNum] < 0)
         {
          if ( %partNum >= %this.maxPart)
            {
             //echo("set sign & stop");
             %this.sign = %sign * -1;
             %this.p[%partNum] = mabs(%this.p[%partNum]);
            }
          else
            {
             //echo("borrow");
             %this.p[%partNum+1]--;
             %this.p[%partNum] += 100000;
            }
          if ( %this.p[%partNum] < 0 )
            {
             AddLog(0, "Error in subtraction" , %aNumber, 1);
             error("Error in subtraction " @ %aNumber);
             %this.dump();
           }
         }
       %partNum++;
      }
    //%this.dump();
    %this.changed=true;
    return %this;
   }

function BigNumber::Display(%this)
   {
    if ( %this.changed)
      {
       %this.display = "";
       for ( %a=1; %a <= %this.maxPart; %a++)
         {
          %str = "00000" @ %this.p[%a];

          %this.display = getSubStr(%str, strlen(%str)-5, 5) @ %this.display;
         }
       while ( getSubStr(%this.display,0,1) $= "0")
         {
          %this.display = getSubStr(%this.display, 1, strlen(%this.display)-1);
         }
       %len = strlen(%this.display);
       if ( %len < 1)
         {
          %this.display="0";
          %this.sign=1;
         }
       if ( %len < 6)
         {
          %this.maxPart=1;
         }

       if ( %this.sign == -1)
         {
          %this.display = "-"@ %this.display;
         }
       %this.changed=false;
      }
    return %this.display;
   }

function BigNumber::LessThan(%this, %aNumber)
   {
    // return true if %this < %aNumber
    %aNumber = expandNumber(%aNumber);
    %s = getSubStr(%aNumber, 0, 1);
    if ( %this.sign== -1)
      {
       if ( %s !$= "-")
         {
          //echo("less than1: true");
          return true;
         }
      }
    if ( %this.sign == 1)
      {
       if ( %s $= "-")
         {
          //echo("less than2: false");
          return false;
         }
      }
    if ( strlen(%aNumber) < 5)
      {
       if ( %this.maxPart == 1)
         {
          //echo("checking3: " @ %this.p1 @ " < " @ %aNumber);
          return %this.p1 < %aNumber;
         }
       else
         {
          //echo("check4: " @%this.display @ " < " @%aNumber);
          return %this.display < %aNumber;
         }
      }

    return %this.LessThanABS(%aNumber);
   }

// compare absolute values, ignoring the sign
function BigNumber::LessThanABS(%this, %aNumber)
   {
    // return true if %this < %aNumber
    //%aNumber = expandNumber(%aNumber);

    %this.display();
    if ( strlen(%this.display) == strlen(%aNumber) )
      {
       for ( %a=0; %a < strlen(%this.display); %a++)
         {
          %ch1 = getSubStr(%this.display, %a, 1);
          %ch2 = getSubStr(%aNumber, %a, 1);
          if ( %ch1 !$= %ch2)
            {
             //echo("check: " @ %ch1 @ " < " @ %ch2);
             return %ch1 < %ch2;
            }
         }
       //echo("less than5: false");
       return false;
      }

    if ( strlen(%this.display) < strlen(%aNumber))
      {
       //echo("less than6: true");
       return true;
      }
    else
      {
       //echo("less than7: false");
       return false;
      }
   }

function BigNumber::GreaterThan(%this, %aNumber)
   {
    %this.display();
    %aNumber = expandNumber(%aNumber);
    if ( %this.display $= %aNumber)
      {
       return false;
      }
    return !%this.lessThan(%aNumber);
   }

function expandNumber(%aNumber)
   {
    %pos = strPos(%aNumber, "e+");
    if ( %pos == -1)
      {
       return %aNumber;
      }

    if ( getSubStr(%aNumber, 0, 1) $= "-")
      {
       %sign = "-";
       %aNumber = getSubStr(%aNumber, 1, strlen(%aNumber)-1);
      }

    %posDec = strPos(%aNumber, ".");
    %zeroes = "00000000000000";
    //         12345678901234
    //  handle up to e+014

    if ( %posDec == -1 )
      {
       // no decimal pt. so number is like 1e+006
       %newNumber = getSubStr(%aNumber, 0, 1) @ %zeroes;
      }
    else
      {
       %newNumber = getsubStr(%aNumber, 0, 1) @ getSubStr(%aNumber, %posDec+1, %pos-(%posDec+1) ) @ %zeroes;
      }

    %digits = getSubStr(%aNumber, %pos+2, 3);
    %newNumber = getSubStr(%newNumber, 0, %digits+1);
    return %sign@%newNumber;
   }

function AddComma(%aString)
   {
    if (getSubStr(%aString, 0, 1) $= "-")
      return "-" @ AddCommaR(expandNumber(getSubStr(%aString, 1, strlen(%aString)) ) );
    else
      return AddCommaR(expandNumber(%aString) );
   }

function addCommaR(%aString)
  {
   %len = strlen(%aString);
   if ( %len < 4)
     return %aString;

   %last = getSubStr(%aString, %len - 3, 3);
   %first = getSubStr(%aString, 0, %len-3);
   %part = addCommaR( %first );
   return %part @ ","@ %last;
  }

function runTests()
   {
    runTestsAdd();
    runTestsSubtract();
   }

function shouldGet(%from, %to, %oper, %result)
   {
    echo("test: " @ %from SPC %oper SPC %to @ " = " @ %result);
    %b1 = bigNumberFromString(%from);
    if ( %oper $= "+")
      {
       %b1.add(%to);
      }
    if ( %oper $= "-")
      {
       %b1.subtract(%to);
      }
    if ( %b1.display() !$= %result)
      {
       error("Failed: " @ %from SPC %oper SPC %to @ " = " @ %result @ ": " @ %b1.display() );
      }
   }

function runTestsAdd()
   {
    // basic level add
    shouldGet("100", "100", "+", "200");
    shouldGet("100000", "100000", "+", "200000");

    // add negative number
    shouldGet("100", "-50", "+", "50");
    shouldGet("100", "-100", "+", "0");
    shouldGet("100000", "-50000", "+", "50000");
    shouldGet("100000", "-100000", "+", "0");

    // negative number add to positive number
    shouldGet("-100", "50", "+", "-50");
    shouldGet("-100", "100", "+", "0");
    shouldGet("-100000", "50000", "+", "-50000");
    shouldGet("-100000", "100000", "+", "0");

    // negative number add to negative number
    shouldGet("-100", "-50", "+", "-150");
    shouldGet("-100", "-100", "+", "-200");
    shouldGet("-100000", "-50000", "+", "-150000");
    shouldGet("-100000", "-100000", "+", "-200000");

    // swapping signs
    shouldGet("-50", "100", "+", "50");
    shouldGet("50", "-100", "+", "-50");

    shouldGet("-50000", "100000", "+", "50000");
    shouldGet("50000", "-100000", "+", "-50000");
   }

function runTestsSubtract()
   {
    // test basic level subtract
    shouldGet("100", "50", "-", "50");

    // test subtract to zero
    shouldGet("100", "100", "-", "0");

    // test subtract to -1
    shouldGet("100", "101", "-", "-1");

    // test large subtract
    shouldGet("100000", "50000", "-", "50000");

    // test large subtract to zero
    shouldGet("100000", "100000", "-", "0");

    // test large subtract to -1
    shouldGet("100000", "100001", "-", "-1");
   }
