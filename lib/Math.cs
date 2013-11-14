// ---------------------------- //
// Large Integer Math Functions //
// by Ipquarx (BL_ID 9291)		//
// ---------------------------- // ------------ //
// Allows for addition, subtraction,			//
// and multiplication of numbers of any size.	//
// --------------------------------------------	//

$log10 = mLog(10);
$log2 = mLog(2);
$l210 = $log10 * $log2;
$l217 = $log2 * mLog(17);

//Addition
function Math_Add(%num1, %num2)
{
	//Check if we can use torque addition
	if(strLen(%num1 @ %num2) < 7)
		return %num1 + %num2;
	
	//Account for sign rules
	if(%num1 >= 0 && %num2 >= 0)
		return stringAdd(%num1, %num2);
	if(%num1 < 0 && %num2 >= 0)
		return stringSub(%num2, switchS(%num1));
	if(%num1 < 0 && %num2 < 0)
		return "-" @ stringAdd(switchS(%num1), switchS(%num2));
	if(%num1 >= 0 && %num2 < 0)
		return stringSub(%num1, switchS(%num2));
	
	//dont know when this would happen but whatev
	return stringadd(%num1,%num2);
}

//Subtraction
function Math_Subtract(%num1, %num2)
{
	//Check if we can use torque subtraction
	if(strLen(%num1 @ %num2) < 7)
		return %num1 - %num2;
	
	//Account for sign rules
	if(%num1 >= 0 && %num2 >= 0)
		return stringSub(%num1, %num2);
	if(%num1 >= 0 && %num2 < 0)
		return stringAdd(%num1, switchS(%num2));
	if(%num1 < 0 && %num2 >= 0)
		return "-" @ stringAdd(%num2, switchS(%num1));
	if(%num1 < 0 && %num2 < 0)
		return stringSub(switchS(%num2), switchS(%num1));
	
	//don't know how this would happen, but ok
	return "Error";
}

//Multiplication
//0 for maxplaces means no limit, -1 means no decimal places
function Math_Multiply(%num1,%num2, %maxplaces)
{
	//Check if we can use torque multiplication
	if((%a=strLen(%num1 @ %num2)) < 7)
		return %num1 * %num2;
	
	//Here's a symbol you don't see everyday, the caret, AKA the XOR operator.
	if(%num1 < 0 ^ %num2 < 0)
		%ans = "-";
	if(%Num1 < 0)
		%Num1 = switchS(%Num1);
	if(%Num2 < 0)
		%Num2 = switchS(%Num2);
	
	%num1 = cleanNumber(%num1); %num2 = cleanNumber(%num2);
	
	if(%maxplaces == 0)
		%maxplaces=-2;
	
	%a = getPlaces(%num1); %b = getPlaces(%num2);
	%c = getMax(%a, %b);
	
	if(%c != 0)
	{
		%places = %a + %b;
		%num1 = cleanNumber(strReplace(%num1, ".", ""));
		%num2 = cleanNumber(strReplace(%num2, ".", ""));
	}
	
	if(%a - strLen(%num2) < 40 && %a - strLen(%num1) < 40)
		%ans = %ans @ strMul(%num1, %num2);
	else
		%ans = %ans @ Karat(%num1, %num2);
	
	if(%c != 0)
	{
		%ans = cleanNumber(placeDecimal(%ans, %places));
		if(%maxplaces != -1)
		{
			if(%maxplaces > 0)
				%ans = getIntPart(%ans) @ "." @ getSubStr(getDecPart(%ans), 0, %maxplaces);
		}
		else
			%ans = getIntPart(%ans);
	}
		
	return %ans;
}

function Math_Pow(%num1, %num2)
{
	if(%num2 < 0)
		return 0;
	if(%num2 == 0 || %num1 == 1)
		return 1;
	
	return expon(%num1, %num2);
}

//This function switches the sign on a number.
function switchS(%a)
{
	if(%a < 0)
		return getSubStr(%a, 1, strLen(%a));
	return "-" @ %a;
}

//decimal shift right
function shiftRight(%a, %b)
{
	return (%c = strLen(%a)) > %b ? getSubStr(%a, 0, %c-%b) : "0"; 
}

//decimal shift left
function shiftLeft(%f,%a)
{
	if(%f$="0")return%f;
	%b = ~~(%a/32);
	
	%d="00000000000000000000000000000000";
	for(%b = %b; %b > 0; %b--)
		%c = %c @ %d;
	
	return (%e=%a%32) ? %f @ %c @ getSubStr(%d, 0, %e) : %f @ %c;
}

function strMul(%Num1,%Num2)
{
	if(%Num1 < 0)
		%Num1 = switchS(%Num1);
	if(%Num2 < 0)
		%Num2 = switchS(%Num2);
	
	%Len1 = strlen(%Num1);
	%Len2 = strlen(%Num2);
	
	for(%a = %Len2-1; %a >= 0; %a--)
		for(%b = %Len1-1; %b >= 0; %b--)
			%Products[%a+%b] += getSubStr(%Num2,%a,1)*getSubStr(%Num1,%b,1);
	
	%MaxColumn = %Len1 + %Len2 + strlen(%Products0) - 2;
	%MaxUse = %MaxColumn - %Len1 - %Len2 + 2;
	
	for(%a = %Len2 + %Len1 - 2; %a >= 0; %a--)
	{
		%x = strLen(%Products[%a]);
		for(%b = strlen(%Products[%a]) - 1; %b >= 0; %b--)
			%Digits[%MaxUse + %a - %x + %b] += getSubStr(%Products[%a], %b, 1);
	}
	
	for(%a=%MaxColumn-1;%a>=0;%a--)
	{
		%Temp = %Digits[%a] + %Carry;
		if(%Temp > 9 && %a != 0)
		{
			%Carry = getSubStr(%Temp, 0, strlen(%Temp) - 1);
			%Temp = getSubStr(%Temp, strlen(%Temp) - 1, strlen(%Temp));
		}
		else
			%Carry = 0;
		%Result = %Temp @ %Result;
	}
	
	return %Result;
}

//Karatsuba multiplication algorithm
//This is faster than the classic multiplication for most numbers greater than 40 digits long.
function Karat(%num1,%num2)
{
	%len1 = strLen(%num1); %len2 = strLen(%num2);
	if(%len1 + %len2 <= 40 || %len1 < 5 || %len2 < 5)
		return strMul(%num1, %num2);
	%m = mCeil(getMax(%len1, %len2) / 2);
	%y=%len1-%m;
	%z=%len2-%m;
	if(%num1 $= "0" || %num2 $= "0")
		return "0";
	if(%len1 <= %m)
	{
		%x0 = %num1;
		%x1 = "0";
		%y0 = getSubStr(%num2, %z, %len2);
		%a = %len2 % %m;
		if(%a == 0)
			%y1 = getSubStr(%num2, 0, %m);
		else
			%y1 = getSubStr(%num2, 0, %a);
	}
	else if(%len2 <= %m)
	{
		%y0 = %num2;
		%y1 = "0";
		%x0 = getSubStr(%num1, %y, %len1);
		%a = %len1 % %m;
		if(%a == 0)
			%x1 = getSubStr(%num1, 0, %m);
		else
			%x1 = getSubStr(%num1, 0, %a);
	}
	else
	{
		%x0 = getSubStr(%num1, %y, %len1);
		%a = %len1 % %m;
		if(%a == 0)
			%x1 = getSubStr(%num1, 0, %m);
		else
			%x1 = getSubStr(%num1, 0, %a);
		%y0 = getSubStr(%num2, %z, %len2);
		%a = %len2 % %m;
		if(%a == 0)
			%y1 = getSubStr(%num2, 0, %m);
		else
			%y1 = getSubStr(%num2, 0, %a);
	}
	%z0 = Karat(%x0, %y0);
	%z2 = Karat(%x1, %y1);
	%z1 = stringSub(strMul(intadd(%x1, %x0), intadd(%y1, %y0)), intAdd(%z0, %z2));
	%a = shiftLeft("", %m);
	return strReplace(lTrim(strReplace(intAdd(intAdd(%z2 @ %a, %z1) @ %a, %z0), "0", " ")), " ", "0");
}

function stringAdd(%num1, %num2)
{
	%a = getDecimal(%num1); %b = getDecimal(%num2);
	%decPlace = getmax(%a, %b);
	if(%decPlace != -1)
	{
		if(%a == -1 && %b != -1)
			%num1 = %num1 @ ".0";
		else if(%b == -1 && %a != -1)
			%num2 = %num2 @ ".0";
		%x = equ0sd(%num1, %num2);
		%num1 = strreplace(getWord(%x, 0), ".", "");
		%num2 = strreplace(getWord(%x, 1), ".", "");
	}
	else
	{
		%x = equ0s(%num1, %num2);
		%num1 = getWord(%x, 0);
		%num2 = getWord(%x, 1);
	}
	for(%a=0;%a<strLen(%num1);%a++)
	{
		%start[%a] = getSubStr(%num1, %a, 1);
		%adder[%a] = getSubStr(%num2, %a, 1);
	}
	%Length = strLen(%num1);
	for(%a = %Length - 1; %a >= 0; %a--)
	{
		%res = %start[%a] + %adder[%a] + %Carry;
		if(%res > 9 && %a != 0)
		{
			%Carry = 1;
			%Ans[%a] = %res - 10;
			continue;
		}
		if(%res < 10)
			%Carry = 0;
		%Ans[%a] = %res;
	}
	for(%a = 0; %a < %length; %a++)
	{
		if(%a == %decPlace)
		{
			%Answer = %Answer @ "." @ %Ans[%a];
			continue;
		}
		%Answer = %Answer @ %Ans[%a];
	}
	if(%decplace > 1)
		%Answer = stripend0s(%Answer);
	return %Answer;
}

function stringSub(%num1, %num2)
{
	%a = getDecimal(%num1); %b = getDecimal(%num2);
	%decPlace = getmax(%a, %b);
	if(%decPlace != -1)
	{
		if(%a == -1 && %b != -1)
			%num1 = %num1 @ ".0";
		else if(%b == -1 && %a != -1)
			%num2 = %num2 @ ".0";
		%x = equ0sd(%num1, %num2);
		%num1 = strreplace(getWord(%x, 0), ".", "");
		%num2 = strreplace(getWord(%x, 1), ".", "");
	}
	else
	{
		%x = equ0s(%num1, %num2);
		%num1 = getWord(%x, 0);
		%num2 = getWord(%x, 1);
	}
	for(%a=0;%a<strLen(%num1);%a++)
	{
		%start[%a]=getSubStr(%num1,%a,1);
		%subtractor[%a]=getSubStr(%num2,%a,1);
	}
	if(%num1 < %num2)
		return "-" @ stringSub(%num2, %num1, %x);
	if(%num1 $= %num2)
		return "0";
	%Length = strLen(%num1);
	for(%a = %Length - 1; %a >= 0; %a--)
	{
		%res = %start[%a] - %subtractor[%a];
		if(%res < 0)
		{
			for(%b=%a-1;%b>=0;%b--)
			{
				if(%start[%b] - %subtractor[%b] > 0)
				{
					%start[%b] -= 1;
					for(%c=%b + 1;%c<%a;%c++)
						%start[%c] += 9;
					%start[%a] += 10;
					break;
				}
			}
			%res = %start[%a] - %subtractor[%a];
		}
		%Ans[%a] = %res;
	}
	%trim = true;
	for(%a = 0; %a < %length; %a++)
	{
		if(%Ans[%a] == 0 && %trim == true && %a != %decPlace - 1)
			continue;
		if(%a == %decPlace)
		{
			%Answer = %Answer @ "." @ %Ans[%a];
			continue;
		}
		%Answer = %Answer @ %Ans[%a];
		%trim = false;
	}
	if(%decplace > 1)
		%Answer = stripend0s(%Answer);
	return %Answer;
}

//This function equalises the length of two numbers by adding zeroes behind the shorter one.
function equ0s(%num1, %num2, %mod)
{
	%x = strLen(%num1); %y = strLen(%num2);
	if(!%mod)
	{
		if(%x < %y)
			%num1 = shiftLeft("", %y - %x) @ %num1;
		else if(%x > %y)
			%num2 = shiftLeft("", %x - %y) @ %num2;
	}
	else
	{
		if(%x < %y)
			%num1 = %num1 @ shiftLeft("", %y - %x);
		else if(%x > %y)
			%num2 = %num2 @ shiftLeft("", %x - %y);
	}
	return %num1 SPC %num2;
}


function expon(%a, %b, %d)
{
	if(%b == 0)
		return 1;
	else if(%b < 0)
		return expon(1/%a, -1 * %b, %d++);
	else if(%b % 2 == 1)
	{
		%c = expon(%a, (%b - 1) / 2, %d++);
		return Math_Multiply(%a, Math_Multiply(%c, %c));
	}
	else if(%b % 2 == 0)
	{
		%c = expon(%a, %b / 2, %d++);
		return Math_Multiply(%c, %c);
	}
}

function stripend0s(%i)
{
	if(%i $= "")
		return"";
	for(%a=0;%a<strLen(%i);%a++)
		%i[%a] = getSubStr(%i, %a, 1);
	%trim = true;
	for(%a=strLen(%i)-1;%a>-1;%a--)
	{
		if(%trim == true && %i[%a] $= "0")
		{
			%i[%a] = "";
			continue;
		}
		else if(%trim == true && %i[%a] $= ".")
		{
			%i[%a] = "";
			continue;
		}
		%trim = false;
	}
	for(%a=0;%a<strLen(%i);%a++)
		%b = %b @ %i[%a];
	return %b;
}

function getIntPart(%i)
{
	if(strpos(%i, ".") == -1)
		return %i;
	return getSubStr(%i, 0, strLen(%i) - strLen(strchr(%i, ".")));
}

function getDecPart(%i)
{
	if(strPos(%i, ".") == -1)
		return"";
	return getSubStr(strChr(%i, "."), 1, 99999);
}

function getDecimal(%i)
{
	return strpos(%i, ".");
}

function equ0sd(%num1, %num2)
{
	%a = getIntPart(%num1); %b = getIntPart(%num2);
	%c = getDecPart(%num1); %d = getDecPart(%num2);
	%e = equ0s(%a, %b);
	%f = equ0s(%c, %d, 1);
	return getWord(%e, 0) @ "." @ getWord(%f, 0) @ " " @ getWord(%e, 1) @ "." @ getWord(%f, 1);
}

//Equivalent to multiplying by 10^-%place
function placeDecimal(%num, %place)
{
	if(strPos(%num, ".") != -1 || %place == 0)
		return %num;
	%log = strLen(%num);
	%pos = %log - %place;
	if(%pos <= 0)
	{
		%start = 0;
		%end = shiftLeft("", -%pos) @ %num;
	}
	else
	{
		%start = getSubStr(%num, 0, %pos);
		%end = getSubStr(%num, %pos, 9999);
	}
	
	return %start @ "." @ %end;
}

function getPlaces(%num)
{
	if(strPos(%num, ".") == -1)
		return 0;
	%num = stripend0s(%num);
	return getMax(strLen(strChr(%num, ".")) - 1, 0);
}

function cleanNumber(%num)
{
	%a = strReplace(lTrim(strReplace(getIntPart(%num), "0", " ")), " ", "0");
	
	%b = stripend0s(getDecPart(%num));
	if(%a $= "" && %b !$= "")
		%a = 0;
	return %a @ (%b !$= "" ? "." @ %b: "");
}

//integers only for now
function Math_Divide(%n, %d, %q)
{
	%qo = %q;
	if(%qo == 0)
		%qo=-1;
	%aa = strLen(getIntPart(%n));
	%bb = strLen(getIntPart(%d));
	%xx = mAbs(%aa - %bb) + 1;
	%q *= 2;
	%q = getMax(%xx,%q);
	
	%e = Math_Multiply(getMax(strLen(getIntPart(%d))-1,1), 3.321928, -1);
	%dd = Math_Multiply(%d, Math_Pow(0.5, %e), %q);
	while(%dd > 1)
	{
		%dd = Math_Multiply(0.5, %dd, %q);
		%e++;
	}
	%n = Math_Multiply(%n, Math_Pow(0.5, %e), %q);
	%x = Math_Subtract(2.823, Math_Multiply(1.882, %dd, %q));
	%x=%dd;
	while(true)
	{
		%x = Math_Multiply(%x, Math_Subtract(2, Math_Multiply(%dd, %x, %q)), %q);
		%z++;
		if(%lastx $= %x)
		{
			if(%z < %xx)
				continue;
			break;
		}
		%lastx = %x;
	}
	return Math_Multiply(%n,%x,%qo);
}

function Math_DivideFloor(%n, %d)
{
	%aa = strLen(getIntPart(%n));
	%bb = strLen(getIntPart(%d));
	%xx = mAbs(%aa - %bb) + 1;
	
	%e = Math_Multiply(getMax(strLen(getIntPart(%d))-1,1), 3.321928, -1);
	%z = Math_Pow(0.5, %e);
	%zz = %e;
	%dd = Math_Multiply(%d, %z, %xx);
	while(%dd > 1)
	{
		%dd = Math_Multiply(0.5, %dd, %xx);
		%e++;
	}
	%n = Math_Multiply(%n, Math_Multiply(%z, Math_Pow(0.5, %e-%zz)), %xx);
	%x = Math_Subtract(2.823, Math_Multiply(1.882, %dd, %xx));
	%x=%dd;
	for(%z = 0; %z < %xx; %z++)
	{
		%x = Math_Multiply(%x, Math_Subtract(2, Math_Multiply(%dd, %x, %xx)), %xx);
		if(%lastx $= %x || %z >= %xx)
			break;
		%lastx = %x;
	}
	return Math_Multiply(%n,%x,-1);
}

function Math_Mod(%a,%b)
{
	return Math_Subtract(%a, Math_Multiply(%b, Math_DivideFloor(%a, %b)));
}

function divider(%numer, %denom, %numDec)
{
	%result = Math_DivideFloor(%numer, %denom);
	%rem = Math_Subtract(%numer, Math_Multiply(%denom, %result)) @ "0";
	echo(%result SPC %rem);
	%result = %result @ ".";
	for (%i=0;%i<%numDec;%i++)
	{
		%x = Math_DivideFloor(%denom, %rem);
		echo(%x);
		%result = %result @ %x;
		%rem = Math_Subtract(%rem, Math_Multiply(%denom, %x)) @ "0";
	}
	return %result;
}