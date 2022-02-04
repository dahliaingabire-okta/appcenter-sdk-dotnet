zipFile=$1
appleExternals=$1
frameworksLocation="${1}/";

unzip ${zipFile} -d ${appleExternals}
cp -LR -f ${frameworksLocation} ${appleExternals}