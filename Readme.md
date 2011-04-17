# StitchIt

A [CommonJS Module](http://wiki.commonjs.org/wiki/Modules/1.1.1) packager for ASP.NET.

## Credits

StitchIt is heavily inspired (almost copied) from [Stitch](https://github.com/sstephenson/stitch) - a great CommonJS module packager for node.js. The name also references [SquishIt](https://github.com/jetheredge/SquishIt), a great JavaScript and CSS bundler for ASP.NET.

## Disclaimer

StitchIt is currently a **work in progress** and is not suitable for using in anything vaguely close to the term *production*. There's a ton of stuff which needs to be added and improved, but don't worry - hopefully it'll be there soon.

## Usage
Using the sample from [CommonJS Module specification](http://wiki.commonjs.org/wiki/Modules/1.1.1#Sample_Code).

* /Scripts/app/math.js
<pre>
	exports.add = function() {
	    var sum = 0, i = 0, args = arguments, l = args.length;
	    while (i < l) {
		sum += args[i++];
	    }
	    return sum;
	};
</pre>

* /Scripts/app/increment.js
<pre>
	var add = require('math').add;

	exports.increment = function(val) {
	    return add(val, 1);
	};
</pre>

* /Scripts/app/program.js
<pre>
	var inc = require('increment').increment;
	var a = 1;
	var b = inc(a);

	console.log(b); // 2
</pre>

* Global.asax
<pre>
	public static void RegisterRoutes(RouteCollection routes)
	{
		...
		routes.StitchIt()
			.RootedAt("~/Scripts/app")
			.PublishAt("app.js");
		...
	}
</pre>

* /Views/Home/Index.cshtml
<pre>
	...
	&lt;script src="/app.js"&gt;&lt;/script&gt;
	&lt;script&gt;
	    stitchIt.require('program');
	&lt;/script&gt;
	...
</pre>
