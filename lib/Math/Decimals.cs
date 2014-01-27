// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Decimal Number Support	//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		 WHAT YOU'RE DOING		---	//
// ------------------------------------	//

//If you don't specify a maximum number of decimal places for division or multiplication, it will assume this.
$Math::DefaultDecimalPlaces = 32;

function FMath_Add(%Num1, %Num2)
{
	//The actual process for adding numbers with decimal parts is no different from adding integers, so we can use the basic functions here.
	//All we do here is equalize the lengths of the fractional parts with zeroes and then send it to the ZMath_Add function.
	
	%part1 = getFractionalPart(%Num1);
	%part2 = getFractionalPart(%Num2);
	
	if(%part1 !$= "" || %part2 !$= "")
	{
		%tmp = equalizeLengths(%part1, %part2, 1);
		%part1 = getWord(%tmp, 0);
		%part2 = getWord(%tmp, 1);
		
		%Num1 = getIntegerPart(%Num1) @ %part1;
		%Num2 = getIntegerPart(%Num2) @ %part2;
		
		%place = strLen(%part1);
	}
	
	%Result = ZMath_Add(%Num1, %Num2);
	
	if(%place+1-1 != 0)
		%Result = placeDecimal(%Result, %place);
	
	if(%Result $= "-0")
		return "0";
	
	if((%l = strLen(%Result)) > 1)
	{
		if(%l > 2)
		{
			if(getSubStr(%Result, 0, 2) $= "-.")
				%Result = "-0." @ getSubStr(%Result, 2, 9999);
		}
		else if(getSubStr(%Result, 0, 1) $= ".")
			%Result = "0." @ getSubStr(%Result, 1, 9999);
	}
	
	return %Result;
}

function FMath_Subtract(%Num1, %Num2)
{
	//The same process as with adding, but calling one function differently.
	
	%part1 = getFractionalPart(%Num1);
	%part2 = getFractionalPart(%Num2);
	
	if(%part1 !$= "" || %part2 !$= "")
	{
		%tmp = equalizeLengths(%part1, %part2, 1);
		%part1 = getWord(%tmp, 0);
		%part2 = getWord(%tmp, 1);
		
		%Num1 = getIntegerPart(%Num1) @ %part1;
		%Num2 = getIntegerPart(%Num2) @ %part2;
		
		%place = strLen(%part1);
	}
	
	%Result = ZMath_Subtract(%Num1, %Num2);
	
	if(%place+1-1 != 0)
		%Result = placeDecimal(%Result, %place);
	
	if((%l = strLen(%Result)) > 1)
	{
		if(%l > 2)
		{
			if(getSubStr(%Result, 0, 2) $= "-.")
				%Result = "-0." @ getSubStr(%Result, 2, 9999);
		}
		else if(getSubStr(%Result, 0, 1) $= ".")
			%Result = "0." @ getSubStr(%Result, 1, 9999);
	}
	
	return %Result;
}

function FMath_Multiply(%Num1, %Num2, %MaxPrecision)
{
	if(%MaxPrecision $= "")
		%MaxPrecision = $Math::DefaultDecimalPlaces;
	
	
}

function getFractionalPart(%Num)
{
	if((%p = strPos(%Num, ".")) == -1)
		return "";
	
	return getSubStr(%num, %p+1, strLen(%Num));
}

function getIntegerPart(%Num)
{
	if((%p = strPos(%Num, ".")) == -1)
		return %Num;
	
	return getSubStr(%num, 0, %p);
}

function placeDecimal(%Num, %Place)
{
	if(%place < 0)
		return;
	
	%Num = strReplace(%Num, ".", "");
	%Len = strLen(%Num);
	
	return getSubStr(%Num, 0, %Len - %Place) @ "." @ getSubStr(%Num, %Len - %Place, 99999);
}