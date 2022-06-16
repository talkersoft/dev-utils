#!/bin/bash
deploypath="/usr/local/bin/aws-update-creds"
cat << EOM > $deploypath
awscredspath="${HOME}/.aws/credentials"
cat << EOM > \$awscredspath
[default]
aws_access_key_id=\${AWS_ACCESS_KEY_ID}
aws_secret_access_key=\${AWS_SECRET_ACCESS_KEY}
aws_session_token=\${AWS_SESSION_TOKEN}
$(echo EOM)
EOM
chmod +x $deploypath