exec("./lib/Math.cs");

function mod(%a,%b)
{
	return Math_Subtract(%a, Math_Multiply(%b, Math_DivideFloor(%a,%b)));
}

$pubPrime = 23;
$pubBase = 5;
$aliceSecret = 5;
$bobSecret = 15;

$alicePublic = mod(math_pow($pubBase, $aliceSecret), $pubPrime);

echo("Alice Public:" SPC $alicePublic);

$bobPublic = mod(math_pow($pubBase, $bobSecret), $pubPrime);

echo("Bob Public:" SPC $bobPublic);

$secret = mod(math_pow($bobPublic, $aliceSecret), $pubPrime);

echo("Secret Num:" SPC $secret);

//function isPrime(%num)
//{
	//if num % 2 == 0, false
	//if num + 1 % 6 == 0 or num - 1 % 6 == 0, continue. otherwise false.
	//loop through all odd numbers less than the square root of num, if divisible by any, false
	//return true
//}