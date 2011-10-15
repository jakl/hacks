#!/bin/sh
alias search='aptitude search'
alias show='aptitude show'
alias instal='sudo aptitude install'
alias remove='sudo aptitude remove'
alias update='sudo aptitude update'
alias upgrade='sudo aptitude safe-upgrade'

alias ls='ls -F --color=auto'
alias la="ls -a | grep '^\.' | column"
alias ff='find . -type f -iname'
alias acp='ack -i --perl'
alias what='ps -e | grep -i'
alias bc='bc -l'
alias s='screen -DR'
alias gs='git status'
alias gp='git pull --rebase'
alias gd='git diff --cached --color -b -w'
alias gl='git log --graph --pretty=format:"%Cred%h%Creset -%C(yellow)%d%Creset %s %Cgreen(%cr) %C(bold blue)<%an>%Creset" --abbrev-commit --date=relative'
alias gsh='git show --date=relative --color'

alias ..='cd ..'
alias ...='cd ../..'
alias ....='cd ../../..'
alias .....='cd ../../../..'
alias ......='cd ../../../../..'

#Wifi Hacking Much?
alias am='sudo airmon-ng start wlan0'

when() {
  history | grep -i $1 | grep -v when
}


if [ -f ~/.my_aliases ]; then
  . ~/.my_aliases
fi

if [ -f ~/.genome_aliases ]; then
  . ~/.genome_aliases
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
export PS1="\$(whoami)@\$(get_box):\$(get_dir)\$(parse_git_branch)$ "
