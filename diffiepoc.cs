exec("./lib/Math.cs");
exec("./src/prime.cs");

$pubPrime = 23;
$pubBase = 5;
$aliceSecret = 5;
$bobSecret = 15;

echo("Secret Num:" SPC $secret);function Math_fastExpMod(%x, %e, %m) // this is the basic function, it needs to be implemented utilizing APA (Abritrary Precision Arithmetic).
{
	%y = "1";

	%z = %x;

	while(alessthanb("0", %e))
	{
		if(Math_Mod(%e ,"2") !$= "0")
		{
			%y = Math_Mod(Math_Multiply(%y, %z), %m);
		}

		%z = Math_Mod(Math_Multiply(%z, %z), %m);

		%e = Math_DivideFloor(%e, 2);
	}

	return %y;
}

$alicePublic = Math_fastExpMod($pubBase, $aliceSecret, $pubPrime);

echo("Alice Public:" SPC $alicePublic);

$bobPublic = Math_fastExpMod($pubBase, $bobSecret, $pubPrime);

echo("Bob Public:" SPC $bobPublic);

$secret = Math_fastExpMod($bobPublic, $aliceSecret, $pubPrime);
