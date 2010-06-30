use strict;
use warnings;
use subs qw(oldgcd gcd lcm);

my $i = 0;#count loops or recursions
sub oldgcd{
	my $a = shift, $b=shift;
	return $a unless $b%$a;
	for (reverse(0..$a/2)){
		$i++;
		return $_ unless ($b%$_ || $a%$_);
	}
}

sub gcd{
	my $a = shift, $b = shift;
	$i++;
	return $b if($a == 0);
	gcd($b%$a,$a);
}

sub lcm{
	my $a = shift, $b = shift;
	$i++;
	return $a*$b/gcd($a,$b);
}

my $a = 611;
my $b = 611;

#make $a the smaller
if($b < $a){
	my $temp = $a;
	$a = $b;
	$b = $temp;
}
print "GCD: ".gcd($a,$b);
print "\nDone in $i hops"; $i=0;
print "\nLCM: ".lcm($a,$b);
print "\nDone in $i hops"; $i=0;