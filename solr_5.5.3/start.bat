rd /s /q server\solr\core0
rd /s /q server\solr\core1

mkdir server\solr\core0
echo name=core0> server\solr\core0\core.properties
echo dataDir=data>> server\solr\core0\core.properties
mkdir server\solr\core1
echo name=core1> server\solr\core1\core.properties
echo dataDir=data>> server\solr\core1\core.properties

xcopy core_template server\solr\core0 /E /Y /R
xcopy core_template server\solr\core1 /E /Y /R
start\solr.cmd start