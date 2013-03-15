#!/bin/sh
alias search='aptitude search'
alias show='aptitude show'
alias instal='sudo apt-get install'
alias remove='sudo apt-get remove'
alias update='sudo apt-get update'
alias upgrade='sudo apt-get dist-upgrade'

alias v='vi'
alias c='cd'
alias l='ls'
alias ls='ls -hF --group-directories-first --color=auto --sort=extension'
alias la='ls -a | grep '^\.' | column'
alias ff='find . -type f -iname'
alias acp='ack -i --perl'
alias what='ps -e | grep -i'
alias bc='bc -l'
alias gs='git status'
alias g='gs'
alias ga='git add'
alias gc='git commit'
alias gp='git pull --ff-only --all'
alias gr='git fetch && git rebase origin/master'
alias gf='git fetch'
alias gds='git diff --staged --color -w'
alias gd='git diff --color -w'
alias gl='git log --graph --pretty=format:"%Cred%h%Creset -%C(yellow)%d%Creset %s %Cgreen(%cr) %C(bold blue)<%an>%Creset" --abbrev-commit --date=relative'
alias glm='gl --author=koval'
alias gph='git push origin HEAD'
alias gsh='git show --date=relative --color'
alias gch='git checkout'
alias flushdns='dscacheutil -flushcache'
alias avg="awk '{a=a+\$1}END{print a/NR}';"
alias nr='repl.history'
alias irc='irssi -n jakl -c irc.freenode.com'

alias ..='cd ..'
alias ...='cd ../..'
alias ....='cd ../../..'
alias .....='cd ../../../..'
alias ......='cd ../../../../..'

LESS=-iMXR
COLORFGBG="default;default" #for transparant mutt background

when() {
  history | grep -i $1 | grep -v when
}


if [ -f ~/.my_aliases ]; then
  . ~/.my_aliases
fi

GIT_PS1_SHOWDIRTYSTATE=1
parse_git_branch() {
  git branch 2> /dev/null | sed -e '/^[^*]/d' -e 's/* \(.*\)/\ (\1)/'
}
get_dir() {
  pwd | sed -e 's/.*\///'
}
get_box() {
    uname -n | sed -e 's/\..*//'
}

#make the input line in the terminal only show the deepest dir and git info
PS1="\$(whoami)@\$(get_box):\$(get_dir)\$(parse_git_branch)$ "
PS1="\[\033[G\]$PS1" # left align prompt, WARNING: will overwrite output that doesn't end in a newline

HISTSIZE=100000
HISTFILESIZE=100000

tunnel () {
  if [ $# = 4 ]; then
    LOCAL_PORT=$1
    HOST_NAME=$2
    REMOTE_PORT=$3
    PROXY_HOST=$4

    if [ -z "`nc -z localhost $LOCAL_PORT`" ]; then
      ssh -N -f -L$LOCAL_PORT:$HOST_NAME:$REMOTE_PORT -l $USER $PROXY_HOST
    else
      echo "Port $LOCAL_PORT is already in use"
    fi
  else
    echo "tunnel usage:"
    echo "   tunnel local_port host_name remote_port proxy_host"
  fi
}

repeat () {
  while true; do $@; sleep 1s; done;
}

toggletouchscreen () {
  id=`xinput | grep -i touchscreen | sed 's/.*id=\(..\).*/\1/'`
  toggle_value=`xinput list-props $id | grep Enabled | tail -c2 | perl -ne '$_ == 1 ? print 0 : print 1'`
  xinput set-prop $id 'Device Enabled' $toggle_value
}

expandurl () { #find the final landing page of a short url like t.co/UgSnleeKua
  curl -sIL $1 | grep ^Location: | tail -n1 | sed 's/Location: //'
}
