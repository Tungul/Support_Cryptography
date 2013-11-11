// ---------------------------- //
// Large Integer Math Functions //
// by Ipquarx (BL_ID 9291)		//
// ---------------------------- // ----------- //
// Allows for addition, subtraction,		   //
// and multiplication of integers of any size. //
// ------------------------------------------- //

//Addition
function Add(%num1, %num2)
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
function Subtract(%num1, %num2)
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
function Multiply(%num1,%num2)
{
	//Check if we can use torque multiplication
	if(strLen(%num1 @ %num2) < 7)
		return %num1 * %num2;
	
	//Here's a symbol you don't see everyday, the caret, AKA the XOR operator.
	//This check will return true
	if(%num1 < 0 ^ %num2 < 0)
		return "-" @ strMul(%num1, %num2);
	
	return strMul(%num1,%num2);
}

//This function switches the sign on a number.
function switchS(%a)
{
	if(%a < 0)
		return getSubStr(%a, 1, strLen(%a));
	return "-" @ %a;
}

//This function is the home of the multiplication algorithm.
function strMul(%Num1,%Num2)
{
	if(%Num1 < 0)
		%Num1 = switchS(%Num1);
	if(%Num2 < 0)
		%Num2 = switchS(%Num2);
	
	%Len1 = strlen(%Num1);
	%Len2 = strlen(%Num2);
	
	//This part on used to be a LOT more complicated and unnessescary.
	for(%a = %Len2-1; %a >= 0; %a--)
		for(%b = %Len1-1; %b >= 0; %b--)
			%Products[%a+%b] += getSubStr(%Num2,%a,1)*getSubStr(%Num1,%b,1);
	
	%Column = %Len1 + %Len2 + strlen(%Products0) - 2;
	
	for(%a = %Len2 + %Len1 - 2; %a >= 0; %a--)
		for(%b = strlen(%Products[%a]) - 1; %b >= 0; %b--)
			%Digits[%MaxColumn + %a - strLen(%Products[%a]) - %Len2 - %Len1 + %b + 2] += getSubStr(%Products[%a], %b, 1);
	
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

function intAdd(%num1,%num2)
{
	%l1=strLen(%num1)-1;
	%l2=strLen(%num2)-1;
	%f=getMax(%l1,%l2);
	for(%a=%f;%a>=0;%a--)
	{
		%d = %l1 - (%f-%a);
		%e = %l2-(%f-%a);
		%b = %d >=0? getSubStr(%Num1,%d,1) : "0";
		%c = %e>=0 ? getSubStr(%Num2,%e,1) : "0";
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
	
	//I know, I know. I can hear you screaming through your monitor
	//"BUT IPQ! TORQUE OPERATORS DON'T WORK FOR HUGE NUMBERS!"
	//But you're wrong. They do. do echo(2309862398762935723 == 2309862398762935723); in your console and find out for yourself.
	if(%num1 == %num2)
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
	if(strLen(%num1) < strLen(%num2))
	{
		%b = strLen(%num2) - strLen(%num1);
		for(%a=0;%a<%b;%a++)
			%num1 = "0" @ %num1;
	}
	else if(strLen(%num1) > strLen(%num2))
	{
		%b = strLen(%num1) - strLen(%num2);
		for(%a=0;%a<%b;%a++)
			%num2 = "0" @ %num2;
	}
	return %num1 SPC %num2;
}