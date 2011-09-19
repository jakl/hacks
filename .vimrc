set number "line numbers
set tabstop=4 "spaces for a tab
set shiftwidth=4 "spaces for indent
set softtabstop=4 "backspace deletes spaces that filled for tab
set expandtab "use spaces rather than tabs
set smartindent "indents after {
set autoindent "keep the current indent for new lines
filetype on "detect filetype
filetype indent on "indent based on filetype
filetype plugin on "load related plugins
syntax enable "Local scope syntax highlighting
set ignorecase "ignore case searches
set incsearch "search while typing
set hlsearch  "highlight searches
set pastetoggle=<F2> "f2 for paste mode, disabling indent
set mouse=a "mouse compatability
set autoread "reload changed files when unedited
set linebreak "wrap lines at natural word dividers
set title "set terminal title to file name
set nowrapscan "don't wrap back to top searches
set nowrap "don't wrap long lines of text. ':set wrap' to reenable
colorscheme elflord "no dark blue, and pretty syntax colors
let g:LargeFile=80 "don't syntax highlight enourmous files
autocmd BufWritePre * :%s/\s\+$//e
"call pathogen#infect() "autoload vim modules installed through git

"complete current word with tab, looking upwards for matches
"use <ctrl>v<tab> if you need an actual tab
inoremap <TAB> <C-P>
"r replaces visual selection with copy/paste buffer
vmap r "_dP
"set foldnestmax=1 "Will be used in an auto-folding script later
