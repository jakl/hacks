#!/usr/bin/perl
use strict;
use warnings;
use subs qw(isprime allprimesupto analyzerange);

sub isprime{
	my $n = shift;
	for(2..sqrt$n){
		return 0 unless($n%$_);
	}
	return 1;
}

sub allprimesupto{
	my $n = shift;
	my @primes;
	for(0..1){
		$primes[$_] = 0;
	}
	for(2..$n){
		$primes[$_] = 1;
	}
	
	for my $prime(2..$n){
		if($primes[$prime]){
			for (my $i = $prime*$prime; $i <= $n; $i+=$prime){
				$primes[$i] = 0;
			}
		}
	}
	my @temp;
	for (0..$#primes){
		push @temp, $_ if $primes[$_];
	}
	
	return @temp;
}

sub analyzerange{
	my $n = shift;
	print "Primes in a certain number range:\n\n";
	my $hops = 0;
	while(1){
		my @primes = allprimesupto($n);
		
		my $i = 0;
		for(0..$n){
			$i++ if $primes[$_];
		}
		print "There are $i primes through $n\n";
		unless ($n){
			print "\nThere were $hops hops";
			exit(0);
		}
		$n = $i;
		$hops ++;
	}
}

#Range to check through is this $n
my $n = 70000;
my @primes = allprimesupto($n);

print $_."," for @primes;
