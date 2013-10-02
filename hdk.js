function addMethod(object, name, fn){
    var old = object[ name ];
    object[ name ] = function(){
        if ( fn.length == arguments.length )
            return fn.apply( this, arguments );
        else if ( typeof old == 'function' )
            return old.apply( this, arguments );
    };
}

function HDK(rootUrl) {
	this.rootUrl = rootUrl;
}

addMethod(HDK.prototype, "fetch", function(callback) {
	this.fetch(this.rootUrl, callback);
});

addMethod(HDK.prototype, "fetch", function(url, callback) {
	$.getJSON(url,function(data) {
		callback($.extend(data,HDK.prototype));
	});
});

addMethod(HDK.prototype, "follow", function(linkName, args, callback) {
	var link = this._links[linkName];
	var href = link.href;
	if(link.templated)	
		href = href.assign(args);

	this.fetch(href,callback);
});