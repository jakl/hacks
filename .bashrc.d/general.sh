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
alias la="ls -a | grep '^\.' | column"
alias ff='find . -type f -iname'
alias acp='ack -i --perl'
alias what='ps -e | grep -i'
alias bc='bc -l'
alias gs='git status'
alias g='gs'
alias ga='git add'
alias gc='git commit'
alias gp='git pull --ff-only --all'
alias gds='git diff --staged --color -w'
alias gd='git diff --color -w'
alias gl='git log --graph --pretty=format:"%Cred%h%Creset -%C(yellow)%d%Creset %s %Cgreen(%cr) %C(bold blue)<%an>%Creset" --abbrev-commit --date=relative --first-parent'
alias glm='gl --author=koval'
alias gph='git push origin head'
alias gsh='git show --date=relative --color'
alias flushdns="dscacheutil -flushcache"
alias avg="awk '{a=a+\$1}END{print a/NR}';"

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

HISTSIZE=100000
HISTFILESIZE=100000
