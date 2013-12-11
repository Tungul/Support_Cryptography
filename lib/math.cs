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
	%b = ~~(%a/32);
	
	%d="00000000000000000000000000000000";
	for(%b = %b; %b > 0; %b--)
		%c = %c @ %d;
	
	return (%e=%a%32) ? %f @ %c @ getSubStr(%d, 0, %e) : %f @ %c;
}

function strmul(%num1, %num2)
{
	if(%num1 < 0)
		%num1 = switchs(%num1);
	if(%num2 < 0)
		%num2 = switchs(%num2);
	
	%x = equ0s(%num1, %num2);
	%num1 = getword(%x, 0);
	%num2 = getword(%x, 1);
	
	%length2 = strLen(%num2);
	%z = -1;
	
	
	for(%a = strLen(%num1) - 3; %a >= 0; %a -= 3)
		%n1[%z++] = getSubStr(%num1, %a, 3);
	
	if(%a > -3)
		%n1[%z++] = getSubStr(%num1, 0, %a + 3);
	
	%tmp2 = 0;
	%z++;
	
	for(%a = %length2 - 3; %a > -3; %a -= 3)
	{
		if(%a >= 0)
			%n2 = getSubStr(%num2, %a, 3);
		else
			%n2 = getSubStr(%num2, 0, %a + 3);
		
		for(%b = 0; %b < %z; %b++)
		{
			%tmp = %n2 * %n1[%b];
			%l = strLen(%tmp);
			
			%tt = %tmp2 + %b;
			
			if(%l < 4)
				%tmps[%tt] += %tmp;
			else
			{
				%l -= 3;
				%tmps[%tt] += getsubstr(%tmp, %l, 3);
				%tmps[%tt + 1] += getsubstr(%tmp, 0, %l);
			}
			
			while(%tmps[%tt] > 999)
			{
				%tmps[%tt+1]++;
				%tmps[%tt] %= 1000;
				%tt++;
			}
		}
		
		%tmps[%tmp2] = $lt[%tmps[%tmp2]];
		%answer1 = %tmps[%tmp2] @ %answer1;
		
		%tmp2++;
	}
	
	%z += %tmp2 - 1;
	for(%a = %tmp2; %a < %z; %a++)
	{
		if(%a + 1 != %z)
			%tmps[%tmp2] = $lt[%tmps[%tmp2]];
		
		%answer1 = %tmps[%a] @ %answer1;
	}
	
	return %answer1;
}

//Karatsuba multiplication algorithm
//This is faster than the classic multiplication for most numbers greater than 40 digits long.

function karat(%num1, %num2)
{
	%n = ~~((getmax(strLen(%num1), strLen(%num2)) + 1) / 2);
	
	if(%n < 150)
		return strMul(%num1,%num2);
	
	%x = shiftLeft("",%n);
	
	%b = shiftRight(%num1, %n);
	%a = stringsub(%num1, %b @ %x);
	%d = shiftRight(%num2, %n);
	%c = stringsub(%num2, %d @ %x);
	
	%ac = karat(%a, %c);
	%bd = karat(%b, $d);
	%abcd = karat(stringadd(%a, %b), stringadd(%c, %d));
	
	return stringadd(%ac, stringadd(%bd @ %x @ %x, stringsub(stringsub(%abcd, %ac), %bd) @ %x));
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

function alessthanb(%a, %b, %c, %d)
{
	if(%c $= "")
		%c = strLen(%a);
	if(%d $= "")
		%d = strLen(%b);
	
	//Only do character-by-character comparisons if lengths are equal
	if(%c != %d)
		return %c < %d;
	
	for(%x = 0; %x < %c; %x+= 5)
	{
		%y = getSubStr(%a, %x, 5); %z = getSubStr(%b, %x, 5);
		if(%y < %z)
			return true;
		else if(%y != %z)
			return false;
	}
	return false;
}

for($zzz=0;$zzz<1000;$zzz++)
{
	$zzx = 3 - strlen($zzz);
	if($zzx != 0)
		$zzz = shiftLeft("", $zzx) @ $zzz;
	$lt[$zzz+1-1] = $zzz;
}
for($zzz=0;$zzz<100000;$zzz++)
{
	$zzx = 5 - strlen($zzz);
	if($zzx != 0)
		$zzz = shiftLeft("", $zzx) @ $zzz;
	$lt2[$zzz+1-1] = $zzz;
}

function Math_TDivide(%num1, %num2)
{
	%q = 0;
	%r = 0;
	fo
}

function Math_Log10Approx(%num)
{
	%l = strLen(%num) - 1;
	return %l + mLog(getsubstr(placeDecimal(%num, %l), 0, 7)) / mLog(10);
}

function Math_Log2Approx(%num)
{
	return 3.322 * Math_Log10Approx(%num);
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
	
	if(%max < 5)
	{
		if(!alessthanb(%field, (%c=%num1+%num2)))
			return %c;
		else
			return %c % %field;
	}
	
	%carry = 0;
	for(%a = %max - 5; 1; 1)
	{
		if(%a >= 0)
		{
			%n1 = getSubStr(%num1, %a, 5);
			%n2 = getSubStr(%num2, %a, 5);
		}
		else
		{
			%x = %a + 5;
			%n1 = getsubstr(%num1, 0, %x);
			%n2 = getsubstr(%num2, 0, %x);
		}
		
		%res = %n1 + %n2 + %carry;
		
		if(%res < 100000)
			%carry = 0;
		else
		{
			%res %= 100000;
			%carry = 1;
		}
		
		%a -= 5;
		
		if(%a > -5)
			%result = $lt2[%res] @ %result;
		else
		{
			%result = %res @ %result;
			break;
		}
	}
	
	if(%field !$= "" && %field)
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
	
	for(%a = %max - 5; %a > -5; %a -= 5)
	{
		if(%a >= 0)
		{
			%n1[%z++] = getSubStr(%num1, %a, 5);
			%n2[%z] = getSubStr(%num2, %a, 5);
		}
		else
		{
			%x = %a + 5;
			%n1[%z++] = getsubstr(%num1, 0, %x);
			%n2[%z] = getsubstr(%num2, 0, %x);
		}
	}
	
	%z++;
	
	for(%a = 0; %a < %z; %a++)
	{
		%res = %n1[%a] - %n2[%a];
		if(%res < 0)
		{
			for(%b = %a + 1; %b < %z; %b++)
			{
				if(%n1[%b] - %n2[%b] > 0)
				{
					%n1[%b]--;
					
					for(%c = %b - 1; %c > %a; %c--)
						%n1[%c] += 99999;
					
					%n1[%a] += 100000;
					break;
				}
			}
			%res = %n1[%a] - %n2[%a];
		}
		
		if(%a + 1 != %z)
			%Ans = $lt2[%res] @ %Ans;
		else
			%Ans = %res @ %Ans;
	}
	
	return %Ans;
}

function FMath_Multiply(%num1, %num2, %field)
{
	%n = %num1;
	%r = 0;
	while(%num2 != 0)
	{
		if(%num2 % 2)
			%r = FMath_Add(%r, %n, %field);
		%n = FMath_Add(%n, %n, %field);
		%num2 = shiftRight(FMath_Multiply2(%num2, 5), 1);
	}
	
	return %r;
}

function FMath_Multiply2(%num1, %num2, %field)
{
	if(strLen(%num1) < 6)
	{
		if(%field !$= "")
			return (%num1 * %num2) % %field;
		return %num1 * %num2;
	}
	%n = %num1;
	%r = 0;
	while(%num2 != 0)
	{
		if(%num2 % 2)
			%r = FMath_Add(%r, %n, %field);
		%n = FMath_Add(%n, %n, %field);
		%num2 = ~~(%num2/2);
	}
	
	return %r;
}