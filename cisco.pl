#!/usr/bin/perl 
#use warnings; #there is a warning in the Net::Telnet::Cisco libary
use strict;
use Net::Telnet::Cisco;

while(<>){
  my $ipRegex = "(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";#matches 0 through 255
  my $ip = $1 if /\b((?:$ipRegex\.){3}$ipRegex)\b/;
  s/$ip//;#delete the ip

  my $userRegex = '\b(.+?)\b';#matches the first entire word
  my $user = $1 if /$userRegex/;
  s/$userRegex//;#delete the user name from $_ buffer

  my $passRegex = '\b(.+)';#matches the second entire word
  my $pass = $1 if /$passRegex/;
  s/$passRegex//;#delete the pass


  my $session = Net::Telnet::Cisco->new(Host => $ip);
  $session->login($user, $pass);
  
  my @output = $session->cmd('show config');
  $session->close;

  print "$ip \n @output \n";
}
