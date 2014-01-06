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
	if(%maxplaces == 0)
		%maxplaces = 500;
	
	//Check if we can use torque multiplication
	if((%aaa=strLen(%num1 @ %num2)) < 6)
	{
		%ans = %num1 * %num2;
		%dont = true;
	}
	else
		%dont = false;
	if(!%dont)
	{
		//Here's a symbol you don't see everyday, the caret, AKA the XOR operator.
		if(%num1 < 0 ^ %num2 < 0)
			%ans = "-";
		if(%Num1 < 0)
			%Num1 = switchS(%Num1);
		if(%Num2 < 0)
			%Num2 = switchS(%Num2);
		
		%num1 = cleanNumber(%num1); %num2 = cleanNumber(%num2);
	}
	
	%a = getPlaces(%num1); %b = getPlaces(%num2);
	%c = getMax(%a, %b);
	if(!%dont)
	{
		if(%c != 0)
		{
			%places = %a + %b;
			%num1 = cleanNumber(strReplace(%num1, ".", ""));
			%num2 = cleanNumber(strReplace(%num2, ".", ""));
		}
		
		if(%aaa <= 300)
			%ans = %ans @ strMul(%num1, %num2);
		else
			%ans = %ans @ Karat(%num1, %num2);	
	}
	
	if(%c != 0)
	{
		if(!%dont)
			%ans = cleanNumber(placeDecimal(%ans, %places));
		if(%maxplaces > 0)
			%ans = cleanNumber(getIntPart(%ans) @ "." @ getSubStr(getDecPart(%ans), 0, %maxplaces));
		else if(%maxplaces != 0)
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
	return getsubstr("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",0,%a);
}

function strmul(%num1, %num2)
{
	%length2 = strLen(%num2);
	%length1 = strLen(%num1);
	%z = -1;
	
	for(%a = %length1 - 4; %a >= 0; %a -= 4)
		%n1[%z++] = getSubStr(%num1, %a, 4) | 0;
	
	if(%a > -4)
		%n1[%z++] = getSubStr(%num1, 0, %a + 4) | 0;
	
	%tmp2 = 0;
	%z++;
	%zz = 0;
	
	for(%a = %length2 - 4; %a > -4; %a -= 4)
	{
		if(%a >= 0)
			%n2 = getSubStr(%num2, %a, 4) | 0;
		else
			%n2 = getSubStr(%num2, 0, %a + 4) | 0;
		
		if(%n2 == 0)
			continue;
		
		for(%b = 0; %b < %z; %b++)
		{
			
			%tmp = %n2 * %n1[%b] | 0;
			
			%l = strLen(%tmp);
			
			%tt = %tmp2 + %b | 0;
			
			if(%l < 5)
			{
				%tmps[%tt] = %tmps[%tt] + %tmp | 0;
				%zz = getmax(%zz, %tt);
			}
			else
			{
				%l -= 4;
				%tmps[%tt] = %tmps[%tt] + getsubstr(%tmp, %l, 4) | 0;
				%tmps[%tt + 1] = %tmps[%tt + 1] + getsubstr(%tmp, 0, %l) | 0;
				%zz = getmax(%zz, %tt+1);
			}
			
			while(%tmps[%tt] >= 10000)
			{
				%tmps[%tt+1] = %tmps[%tt+1] + 1 | 0;
				%tmps[%tt] = %tmps[%tt] - 10000 | 0;
				%tt++;
				%zz = getmax(%zz, %tt);
			}
		}
		
		%answer1 = $lt[%tmps[%tmp2]] @ %answer1;
		
		%tmp2++;
	}
	
	if((%tmps[%zz]|0) != 0)
		%zz++;
	
	for(%a = %tmp2; %a < %zz; %a++)
	{
		if(%a + 1 != %zz)
			%tmps[%a] = $lt[%tmps[%a]];
		
		%answer1 = %tmps[%a] @ %answer1;
	}
	
	return %answer1;
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
		%a = getDecimal(%num1); %b = getDecimal(%num2);
		%decPlace = getmax(%a, %b);
		%num1 = strreplace(getWord(%x, 0), ".", "");
		%num2 = strreplace(getWord(%x, 1), ".", "");
	}
	else
	{
		%x = equ0s(%num1, %num2);
		%num1 = getWord(%x, 0);
		%num2 = getWord(%x, 1);
	}
	%Length = strLen(%num1);
	for(%a=0;%a<%Length;%a++)
	{
		%start[%a] = getSubStr(%num1, %a, 1);
		%adder[%a] = getSubStr(%num2, %a, 1);
	}
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
		%a = getDecimal(%num1); %b = getDecimal(%num2);
		%decPlace = getmax(%a, %b);
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
	%c = (%c=getDecPart(%num1)) $= "" ? 0 : %c; %d = (%d=getDecPart(%num2)) $= "" ? 0 : %d;
	
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
	return %a @ (%b !$= "" ? "." @ %b : "");
}

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
	%xx = mAbs(%aa - %bb) + 2;
	
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
	%x = %b;
	%y = strLen(%a); %z = strLen(%b);
	
	if(alessthanb(%a,%b))
		return %a;
	else if((%aa=~~(3.4*(%y-%z))) > 0)
		%b = Math_Multiply(%b, Math_Pow(2, %aa));
	
	while(!alessthanb(%a, %b))
		%b = Math_Multiply(%b, 2);
	
	while(!alessthanb(%a, %x))
	{
		if(alessthanb(%a,%b))
		{
			%aa = ~~(3.4 * (strLen(%b) - strLen(%a)));
			%b = Math_Multiply(%b, Math_Pow(0.5, getMax(1,%aa)), -1);
			
			while(alessthanb(%a, %b))
				%b = Math_Multiply(%b, 0.5, -1);
		}
		
		%a = Math_Subtract(%a, %b);
	}
	return %a;
}

function divider(%numer, %denom, %numDec)
{
	%result = Math_DivideFloor(%numer, %denom);
	%rem = Math_Subtract(%numer, Math_Multiply(%denom, %result)) @ "0";
	%result = %result @ ".";
	for (%i=0;%i<%numDec;%i++)
	{
		%x = Math_DivideFloor(%denom, %rem);
		%result = %result @ %x;
		%rem = Math_Subtract(%rem, Math_Multiply(%denom, %x)) @ "0";
	}
	return %result;
}

function alessthanb(%a, %b)
{
	if(%a $= %b)
		return false;
	
	%c = strLen(%a);
	%d = strLen(%b);
	
	//Only do character-by-character comparisons if lengths are equal
	if(%c != %d)
		return %c < %d;
	
	for(%x = 0; %x < %c; %x += 9)
	{
		%y = getSubStr(%a, %x, 9) | 0; %z = getSubStr(%b, %x, 9) | 0;
		
		if((%y|0) < %z)
			return true;
		else if((%y|0) > %z)
			return false;
	}
}

for($zzz=0;$zzz<10000;$zzz++)
{
	$zzx = 4 - strlen($zzz);
	if($zzx)
		$zzz = shiftLeft("", $zzx) @ $zzz;
	$lt[$zzz+1-1] = $zzz;
}

function Math_TDivide(%num1, %num2)
{
	
}

//Approximates the base 10 logarithm of the specified arbitrary integer. Good to ~4 decimal places.
function Math_Log10Approx(%num)
{
	%l = strLen(%num) - 1;
	return %l + mLog(getsubstr(placeDecimal(%num, %l), 0, 7)) / mLog(10);
}

//Approximates the base 2 logarithm of the given integer. Good to ~3 decimal places.
function Math_Log2Approx(%num)
{
	return 3.3219 * Math_Log10Approx(%num);
}


//FINITE FIELD MATH METHODS
//USES INTEGERS ONLY

function FMath_Add(%num1, %num2, %field)
{
	%length1 = strLen(%num1);
	%length2 = strLen(%num2);
	%max = getmax(%length1,%length2);
	
	if(%length2 != %length1)
	{
		%x = equ0s(%num1, %num2);
		%num1 = getword(%x, 0);
		%num2 = getword(%x, 1);
	}
	
	if(%max < 6)
	{
		if(!alessthanb(%field, (%c=%num1+%num2)))
			return %c;
		else
			return %c % %field;
	}
	
	%carry = 0;
	for(%a = %max - 9; true; true)
	{
		if(%a >= 0)
		{
			%n1 = getSubStr(%num1, %a, 9) | 0;
			%n2 = getSubStr(%num2, %a, 9) | 0;
		}
		else
		{
			%x = %a + 9;
			%n1 = getsubstr(%num1, 0, %x) | 0;
			%n2 = getsubstr(%num2, 0, %x) | 0;
		}
		
		%res = (((%n1|0) + (%n2|0)) | 0) + (%carry | 0) | 0;
		%l = strlen(%res);
		
		if(%l > 9)
		{
			%res = ((%res|0) - 1000000000) | 0;
			%l = strlen(%res);
			%carry = 1;
		}
		else
			%carry = 0;
		
		%a -= 9;
		
		if(%a > -9)
		{
			%l = 9 - %l;
			if(%l > 0)
				%res = getsubstr("00000000",0, %l) @ %res;
			
			%result = %res @ %result;
		}
		else
		{
			if(%carry == 0)
			{
				%result = %res @ %result;
			}
			else
			{
				%l = 9 - %l;
				if(%l > 0)
					%res = getsubstr("00000000",0, %l) @ %res;
					
				%result = "1" @ %res @ %result;
			}
			
			break;
		}
	}
	
	if(%field !$= "" && %field !$= "0")
	{
		if(%field $= %result)
			return 0;
		
		if(alessthanb(%field, %result))
			return FMath_Subtract(%result, %field);
	}
	
	return %result;
}

function FMath_Subtract(%num1, %num2, %field)
{
	%length1 = strLen(%num1);
	%length2 = strLen(%num2);
	%max = getmax(%length1,%length2);
	
	if(alessthanb(%num1, %num2))
	{
		echo("BLEHHHHHHHHHHH");
		if(%field !$= "")
			return FMath_Subtract(%field, FMath_Subtract(%num2, %num1));
		else
			return "-" @ FMath_Subtract(%num2, %num1);
	}
	
	if(%length1 != %length2)
	{
		%x = equ0s(%num1, %num2);
		%num1 = getword(%x, 0);
		%num2 = getword(%x, 1);
	}
	
	%z = -1;
	
	for(%a = %max - 9; true; %a -= 9)
	{
		if(%a >= 0)
		{
			%n1[%z++] = getSubStr(%num1, %a, 9) | 0;
			%n2[%z] = getSubStr(%num2, %a, 9) | 0;
		}
		else
		{
			%x = %a + 9;
			%n1[%z++] = getsubstr(%num1, 0, %x) | 0;
			%n2[%z] = getsubstr(%num2, 0, %x) | 0;
			break;
		}
	}
	
	%z++;
	
	for(%a = 0; %a < %z; %a++)
	{
		%res = ((%n1[%a]|0) - (%n2[%a]|0)) | 0;
		
		if(%res @ "" < 0)
		{
			for(%b = %a + 1; %b < %z; %b++)
			{
				if((((%n1[%b]|0) - (%n2[%b]|0)) | 0) @ "" > 0)
				{
					%n1[%b] = ((%n1[%b] | 0) - 1) | 0;
					%n1[%a] = ((%n1[%a] | 0) + 1000000000) | 0;
					
					for(%c = %b - 1; %c > %a; %c--)
						%n1[%c] = ((%n1[%c] | 0) + 999999999) | 0;
					
					break;
				}
			}
			%res = ((%n1[%a]|0) - (%n2[%a]|0)) | 0;
		}
		
		if(%a + 1 != %z)
			%Ans = ((%x=9-strlen(%res)) > 0 ? (getsubstr("00000000",0,%x) @ %res) : %res) @ %Ans;
		else
		{
			if(%res != 0)
				%Ans = %res @ %Ans;
			else
				while(getsubstr(%ans, 0, 1) $= "0")
					%ans = getsubstr(%ans, 1, 500);
		}
	}
	
	return %Ans;
}

function FMath_Multiply(%num1, %num2, %field)
{
	%n = %num1;
	%r = 0;
	
	while(%num2 != 0)
	{
		%tmp = getsubstr(%num2, getmax(strlen(%num2) - 5, 0), 5) | 0;
		
		if(%tmp % 2)
			%r = FMath_Add(%r, %n, %field);
		
		%n = FMath_Add(%n, %n, %field);
		%tmp = ~~(%tmp/2);
		if(%tmp % 2)
			%r = FMath_Add(%r, %n, %field);
		
		%n = FMath_Add(%n, %n, %field);
		%tmp = ~~(%tmp/2);
		if(%tmp % 2)
			%r = FMath_Add(%r, %n, %field);
		
		%n = FMath_Add(%n, %n, %field);
		%tmp = ~~(%tmp/2);
		if(%tmp % 2)
			%r = FMath_Add(%r, %n, %field);
		
		%n = FMath_Add(%n, %n, %field);
		%tmp = ~~(%tmp/2);
		if(%tmp % 2)
			%r = FMath_Add(%r, %n, %field);
		
		%n = FMath_Add(%n, %n, %field);
		
		%num2 = FMath_DivideBy32(%num2);
	}
	
	return %r;
}

//Just a little for loop to make some divide by powers of two functions that all work in a single multiplication.
for($x = 1; $x < 6; $x++)
	eval("function FMath_DivideBy" @ mpow(2, $x) @ "(%num) { return strlen(%num) > 5 ? shiftRight(strmul(%num," @ mpow(5, $x) @ ")," @ $x @ ") : ~~(%num/" @ mpow(2, $x) @ "); }");
	
	function generateTable(%num)
{
	$table0 = 0;
	$table1 = %num;
	
	for(%a = 2; %a < 10000; %a++)
		$table[%a] = FMath_Add($table[%a-1], %num);
}

function Math_ConstantMod(%num)
{
	%other = $table1;
	%take = strlen(%other) + 4;
	%r2 = getsubstr(%other, 0, 6);
	%otherl = %take - 4;
	
	while(alessthanb(%other, %num))
	{
		//Get a number to do a modulus on using our multiplication cache
		%tmp = getsubstr(%num, 0, %take);
		%tmpl = strlen(%tmp);
		
		%r1 = getsubstr(%tmp, 0, 6);
		
		//Calculate tmp/num2.
		%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("000000", 0, %tmpl - %otherl)));
		
		//Make sure we're within range of the multiplication cache.
		%xx = 0;
		while(%ratio > 9999)
		{
			%tmp = getsubstr(%tmp, 0, %take-(%xx++));
			%tmpl--;
			%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("000000", 0, %tmpl - %otherl)));
		}
		
		//Calculate the modulus and prefix it to the original number.
		//This way, we're only doing 1 subtraction every 3/4 digits of the number. This makes it very fast.
		%num = FMath_Subtract(%tmp, $table[%ratio]) @ getsubstr(%num, %take - %xx, 500);
	}
	
	return %num;
}

function Math_Mod(%num, %num2)
{
	%take = strlen(%num2) + 5;
	%r2 = getsubstr(%num2, 0, 7);
	%n2l = %take - 5;
	
	while(alessthanb(%num2, %num))
	{
		//Get a temporary
		%tmp = getsubstr(%num, 0, %take);
		%tmpl = strlen(%tmp);
		%r1 = getsubstr(%tmp, 0, 7);
		
		//Calculate tmp/num2.
		%ratio = ~~(%r1 / %r2 * (1 @ getsubstr("0000000000000", 0, %tmpl - %n2l)));
		
		//Calculate the modulus, and prefix it back onto the original number. 
		%num = FMath_Subtract(%tmp, strmul(%num2, %ratio)) @ getsubstr(%num, %take, 500);
	}
	
	return %num;
}

function add(%x, %y)
{
	%a = 1;

	while (%a)
	{
		%a = %x & %y;
		%b = %x ^ %y;
		%x = %a << 1;
		%y = %b;
	}

	return %b;
}