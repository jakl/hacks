"git clone https://github.com/gmarik/vundle.git ~/.vim/bundle/vundle

"Must `apt-get install git` and run above clone! These Bundle commands won't work otherwise.
set rtp+=~/.vim/bundle/vundle
call vundle#rc()
Bundle 'gmarik/vundle'
Bundle 'kchmck/vim-coffee-script'
Bundle 'scrooloose/nerdtree'
Bundle 'fholgado/minibufexpl.vim'
Bundle 'majutsushi/tagbar'
Bundle 'leshill/vim-json'

"mkdir ~/.vim/undodir
set undodir=~/.vim/undodir "persistent undos between editing sessions
set undofile "Doesn't work in ubuntu 10.04

set number "line numbers
set tabstop=2 "spaces for a tab
set shiftwidth=2 "spaces for indent
set softtabstop=2 "backspace deletes spaces that filled for tab
set expandtab "use spaces rather than tabs
set smartindent "indents after {
set autoindent "keep the current indent for new lines
filetype plugin indent on "detect filetype, indent, load respective plugins
syntax enable "Local scope syntax highlighting
set ignorecase "ignore case searches
set incsearch "search while typing
set hlsearch  "highlight searches
set pastetoggle=<F2> "f2 for paste mode, disabling indent
nmap <F3> :set invnumber<CR>
set mouse=a "mouse compatability
set autoread "reload changed files when unedited
set linebreak "wrap lines at natural word dividers
set title "set terminal title to file name
set nowrapscan "don't wrap back to top searches
set nowrap "don't wrap long lines of text. ':set wrap' to reenable
colorscheme elflord "no dark blue, but instead use pretty syntax colors
set fillchars=fold:\ "Don't append hyphens - at the end of folds, use spaces
hi Folded ctermbg=none "Folds respect terminal transparency

autocmd BufWritePre * :call Preserve("%s/\\s\\+$//e") "Delete trailing whitespace on write
function! Preserve(command)
    let _s=@/
    let l = line(".")
    let c = col(".")
    execute a:command
    let @/=_s
    call cursor(l, c)
endfunction


"complete current word with tab, looking upwards for matches
"use ctrl+v tab if you need an actual tab
inoremap <TAB> <C-P>

"r replaces visual selection with yank's buffer
vmap r "_dP

map <leader>t :TagbarToggle<CR>
map <leader>n :NERDTreeToggle<CR>


"Use ctrl+h,j,k,l to move among split buffers
nmap <silent> <C-k> :wincmd k<CR>
nmap <silent> <C-j> :wincmd j<CR>
nmap <silent> <C-h> :wincmd h<CR>
nmap <silent> <C-l> :wincmd l<CR>
nmap <silent> <C-n> :bp<CR>
nmap <silent> <C-m> :bn<CR>

"ctrl+c in visual mode will copy to mac system buffer
vmap <C-c> y:call system("pbcopy", getreg("\""))<CR>

" Search for selected text with *
vnoremap <silent> * :<C-U>
  \let old_reg=getreg('"')<Bar>let old_regtype=getregtype('"')<CR>
  \gvy/<C-R><C-R>=substitute(
  \escape(@", '/\.*$^~['), '\_s\+', '\\_s\\+', 'g')<CR><CR>
  \gV:call setreg('"', old_reg, old_regtype)<CR>
