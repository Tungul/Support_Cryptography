// ---------------------------- //
// Large Integer Math Functions //
// by Ipquarx (BL_ID 9291)		//
// ---------------------------- // ----------- //
// Allows for addition, subtraction,		   //
// and multiplication of integers of any size. //
// ------------------------------------------- //

//Addition
function Math_Add(%num1, %num2)
{
	//Check if we can use torque addition
	if(strLen(%num1 @ %num2) < 7)
		return %num1 + %num2;
	
	//Account for sign rules
	if(%num1 >= 0 && %num2 >= 0)
		return intAdd(%num1, %num2);
	if(%num1 < 0 && %num2 >= 0)
		return stringSub(%num2, switchS(%num1));
	if(%num1 < 0 && %num2 < 0)
		return "-" @ intAdd(switchS(%num1), switchS(%num2));
	if(%num1 >= 0 && %num2 < 0)
		return stringSub(%num1, switchS(%num2));
	
	//dont know when this would happen but whatev
	return intadd(%num1,%num2);
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
		return intAdd(%num1, switchS(%num2));
	if(%num1 < 0 && %num2 >= 0)
		return "-" @ intAdd(%num2, switchS(%num1));
	if(%num1 < 0 && %num2 < 0)
		return stringSub(switchS(%num2), switchS(%num1));
	
	//don't know how this would happen, but ok
	return "Error";
}

//Multiplication
function Math_Multiply(%num1,%num2)
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
	
	//Check if it's better to use classic or karatsuba
	if(%a - strLen(%num2) < 15 && %a - strLen(%num1) < 15)
		return %ans @ strMul(%num1, %num2);
	
	return %ans @ Karat(%num1, %num2);
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

function intAdd(%num1,%num2)
{
	%l1=strLen(%num1)-1;
	%l2=strLen(%num2)-1;
	%f=getMax(%l1,%l2);
	for(%a=%f;%a>=0;%a--)
	{
		%d = %l1 - (%f-%a);
		%e = %l2-(%f-%a);
		%b = %d >=0 ? getSubStr(%Num1,%d,1) : "0";
		%c = %e >=0 ? getSubStr(%Num2,%e,1) : "0";
		%res = %b+%c+%Carry;
		if(%res > 9 && %a != 0)
		{
			%Carry = 1;
			%Answer = %res-10 @ %Answer;
			continue;
		}
		else
			%Carry=0;
		%Answer = %res @ %Answer;
	}
	return %Answer;
}

//This is probably the most unoptimised of all the functions here.
function stringSub(%num1, %num2)
{
	if(%Num1 < 0)
		%Num1 = switchS(%Num1);
	if(%Num2 < 0)
		%Num2 = switchS(%Num2);
	
	//We have to equalise the length of the two strings.
	%a = equ0s(%num1, %num2);
	
	%num1 = getWord(%a, 0);
	%num2 = getWord(%a, 1);
	
	%Count = strLen(%num1);
	
	//We have to put these in arrays so we can account for lookahead carrying.
	for(%a = 0; %a < %Count; %a++)
	{
		%start[%a] = getSubStr(%num1, %a, 1);
		%subtractor[%a] = getSubStr(%num2, %a, 1);
	}
	
	//Account for cases where the result will be negative
	if(%num1 < %num2)
		return "-" @ stringSub(%num2, %num1);
	
	if(!strCmp(%num1, %num2))
		return "0";
	
	%Length = strLen(%num1);
	
	//This is the most complicated part of the script.
	for(%a = %Length - 1; %a >= 0; %a--)
	{
		%res = %start[%a] - %subtractor[%a];
		if(%res < 0)
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
		
		%Answer = %res @ %Answer;
	}
	
	//Sometimes the answer will come out with leading zeroes, so we get rid of those here.
	%Answer = strReplace(lTrim(strReplace(%Answer, "0", " ")), " ", "0");
	
	return %Answer;
}

//This function equalises the length of two numbers by adding zeroes behind the shorter one.
function equ0s(%num1, %num2)
{
	%x = strLen(%num1); %y = strLen(%num2);
	if(%x < %y)
		%num1 = shiftLeft("", %y - %x) @ %num1;
	else if(%x > %y)
		%num2 = shiftLeft("", %x - %y) @ %num2;
	
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