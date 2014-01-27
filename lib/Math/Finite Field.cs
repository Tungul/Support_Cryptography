// ------------------------------------	//
// --- Arbitrary Math Library β 1.0 ---	//
// ------------------------------------	//
// --- Created by Ipquarx (BLID 9291)	//
// --- With assistance/input from:		//
// --- Lugnut	(BLID 16807)			//
// --- Greek2me	(BLID 11902)			//
// --- Xalos	(BLID 11239)			//
// ------------------------------------	//
// --- Module: Finite Field Mathematics	//
// ------------------------------------	//
// --- DON'T MODIFY UNLESS YOU KNOW ---	//
// ---		 WHAT YOU'RE DOING		---	//
// ------------------------------------	//

//Function Purpose:	Performs modular addition on two numbers that are in the given field.
//Function Inputs:	num1: The first number.
//					num2: The number to add to the first.
//					field: The finite field to add in.
//Requisites:		num1 and num2 must both be < field and >= 0.
function QMath_Add(%num1, %num2, %field)
{
	if(%field $= "")
		return;

	if(%field $= "0")
		return "0";

	%result = IMath_Add(%num1, %num2);

	//If the field is the exact same as the result, then the answer is automatically 0.
	if(%field $= %result)
		return 0;

	if(aLessThanb(%field, %result)) //Assuming both inputs were < %field, and the result is > %field, then a simple subtraction of %field will fix it.
		return IMath_Subtract(%result, %field);

	return %result;
}

//Function Purpose:	Performs modular subtraction on two numbers that are in the given field.
//Function Inputs:	num1: The first number.
//					num2: The number to subtract from the first.
//					field: The finite field to work from.
//Requisites:		num1 and num2 must both be < field and >= 0.
function QMath_Subtract(%num1, %num2, %field)
{
	if(%field $= "")
		return;

	if(%field $= "0")
		return "0";

	//We have to do some silly stuff here in order to make the result positive and within the range [0, field).
	if(aLessThanb(%num1, %num2))
		return IMath_Subtract(%field, IMath_Subtract(%num2, %num1));

	return IMath_Subtract(%num1, %num2);
}

function QMath_ModMultiply(%num1, %num2, %num3, %isConstant)
{
	%tmp = IMath_Multiply(%num1, %num2);
	return %isConstant ? CMath_Mod(%tmp, %num3) : ZMath_Mod(%tmp, %num3);
}