#!/bin/bash

# tag with shortened git sha
shortsha=${SOURCE_COMMIT:0:12}
docker tag $IMAGE_NAME $DOCKER_REPO:$shortsha
docker push $DOCKER_REPO:$shortsha

# update latest image tag only if building on master
if [ $SOURCE_BRANCH == 'master' ]; then
    docker push $DOCKER_REPO:latest
fi

# convert 1.2.3 into an array consisting of 1,1.2,1.2.3
vsparts=($(echo $DOCKER_TAG | tr "." "\n"))
vs=''
for (( i=0; i<${#vsparts[@]}; ++i )) do
    if [ ! "$vs" ]; then
        vs=${vsparts[$i]}
    else
        vs="$vs.${vsparts[$i]}"
    fi
    versions[$i]=$vs
done

# update major and minor semver tags (eg 1 and 1.2)
# IMPORTANT: this will overwrite any existing image tags
for (( i=0; i<${#versions[@]}; ++i )) do
    vs=${versions[$i]}
    if [ "$vs" != "$DOCKER_TAG" ]; then
        docker tag $IMAGE_NAME $DOCKER_REPO:$vs
        docker push $DOCKER_REPO:$vs
    fi
done