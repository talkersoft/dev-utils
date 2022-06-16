#!/bin/bash
gittoppath="/usr/local/bin/gittop"
cat << EOM > $gittoppath
#!/bin/bash
git rev-parse --show-toplevel
EOM
chmod +x $gittoppath