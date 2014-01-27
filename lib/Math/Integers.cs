// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Integer Arithmetic		//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		 WHAT YOU'RE DOING		---	//
// ------------------------------------	//

$Math::Factorial0 = 1;
$Math::Factorial1 = 1;
$Math::Factorial2 = 2;
$Math::CalculatedFactorials = 2;

function ZMath_Add(%Num1, %Num2)
{
	%test1 = getsubstr(%Num1, 0, 4) > 0;
	%test2 = getsubstr(%Num2, 0, 4) > 0;

	if(%test1 && %test2)
		return IMath_Add(%Num1, %Num2);

	if(%test1 && !%test2)
		return IMath_Subtract(%Num1, makePositive(%Num2));
	
	if(!%test1 && %test1)
		return IMath_Subtract(%Num2, makePositive(%Num1));
	
	if(!%test1 && !%test2)
		return "-" @ IMath_Add(makePositive(%Num1), makePositive(%Num2));
	
	return "ERROR";
}

function ZMath_Subtract(%Num1, %Num2)
{
	%test1 = getsubstr(%Num1, 0, 4) > 0;
	%test2 = getsubstr(%Num2, 0, 4) > 0;

	
	if(%test1 && %test2)
		return IMath_Subtract(%Num1, %Num2);

	
	if(%test1 && !%test2)
		return IMath_Add(%Num1, makePositive(%Num2));

	if(!%test1 && %test1)
		return "-" @ IMath_Add(%Num2, makePositive(%Num1));

	if(!%test1 && !%test2)
		return IMath_Subtract(makePositive(%Num2), makePositive(%Num1));

	return "ERROR";
}

function ZMath_Multiply(%Num1, %Num2)
{
	%test1 = getsubstr(%Num1, 0, 4) > 0;
	%test2 = getsubstr(%Num2, 0, 4) > 0;

	if(%test1 ^ %test2)
		return "-" @ IMath_Multiply(makePositive(%Num1), makePositive(%Num2));

	return IMath_Multiply(makePositive(%Num1), makePositive(%Num2));
}

function ZMath_Mod(%num, %num2)
{
	if(%num2 $= "0")
		return 0;

	%take = strlen(%num2) + 4;
	%r2 = getsubstr(%num2, 0, 7);
	%n2l = %take - 4;
	%len1 = strLen(%num);

	while(aLessThanb(%num2, %num))
	{
		//Get a temporary
		%tmp = getsubstr(%num, 0, %take);
		%tmpl = strlen(%tmp);
		%r1 = getsubstr(%tmp, 0, 7);

		//Calculate tmp/num2.
		%ratio = (%r1 / %r2 * (1 @ getsubstr("0000", 0, %tmpl - %n2l) | 0)) | 0;

		//Calculate the modulus, and prefix it back onto the original number.
		%num = IMath_Subtract(%tmp, IMath_Multiply(%ratio, %num2)) @ getsubstr(%num, %take, %len1);
	}

	return %num;
}

function ZMath_DivFloor(%num, %num2)
{
	%test1 = getsubstr(%Num, 0, 4) > 0;
	%test2 = getsubstr(%Num2, 0, 4) > 0;

	if(%test1 ^ %test2)
		%Result = "-";

	%num = makePositive(%Num);
	%num2 =	makePositive(%Num2);

	%take = strlen(%num2) + 4;
	%r2 = getsubstr(%num2, 0, 7);
	%n2l = %take - 4;

	while(aLessThanb(%num2, %num))
	{
		//Get a temporary
		%tmp = getsubstr(%num, 0, %take);
		%tmpl = strlen(%tmp);
		%r1 = getsubstr(%tmp, 0, 7);

		//Calculate tmp/num2.
		%ratio = (%r1 / %r2 * (1 @ getsubstr("00000", 0, %tmpl - %n2l) | 0)) | 0;

		//Place the ratio at the end of the result. This quite literally makes up the digits of the quotient.
		%Result = %Result @ %ratio;

		//Calculate the modulus, and prefix it back onto the original number.
		%num = IMath_Subtract(%tmp, IMath_Multiply(%ratio, %num2)) @ getsubstr(%num, %take, %take);
	}

	return %Result !$= "" ? %Result : "0";
}

function ZMath_Factorial(%Num)
{
	if($Math::CalculatedFactorial >= %Num)
		return $Math::Factorial[%Num];
	
	for(%a = $Math::CalculatedFactorial + 1; %a <= %Num; %a++)
		$Math::Factorial[%a] = IMath_Multiply($Math::Factorial[%a-1], %a);
	
	$Math::CalculatedFactorial = %Num;
	
	return $Math::Factorial[%Num];
}