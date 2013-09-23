function Application() {
	var self = this;
	this.viewModelCache = {};
	this.currentViewModel = ko.observable(this.getViewModel('home',HomeViewModel));

	Sammy(function() {
		this.get('#home', function() {
			console.log('Route: Home');
			self.currentViewModel(self.getViewModel('home', HomeViewModel));
		});

		this.get('#places', function() {
			console.log('Route: Places');
			self.currentViewModel(self.getViewModel('places', PlacesViewModel));
		});

		this.get('#place/:id', function() {
			console.log('Route: Place');
			self.getViewModel('place', PlaceViewModel).load(this.params['id']);
			self.currentViewModel(self.getViewModel('place', PlaceViewModel))
		});

		this.get('', function() {
			this.app.runRoute("get", "#home");	
		});
	}).run();
};

Application.prototype.getViewModel = function (name, constructor) {
	if(!this.viewModelCache[name]) {
		this.viewModelCache[name] = new constructor();
	}

	return this.viewModelCache[name];
}

Application.prototype.link = function(data,event) {
	event.preventDefault();
	location.hash = $(event.target).attr('href');	
};

function HomeViewModel() {
	this.template = ko.observable('home');
};

function PlacesViewModel() {	
	this.template = ko.observable('places');

	this.places = ko.observableArray();
	this.keyword = ko.observable();
	this.showError = ko.observable(false);
	this.loading = ko.observable(false);
	this.searchType = ko.observable('eng');
};

PlacesViewModel.prototype.doSearch = function() {
	var self = this;

	if(!this.keyword() || this.keyword().length < 3) {
		this.showError(true);
		return;
	}

	this.loading(true);
	this.showError(false);

	var url = this.searchUrl();
	$.getJSON(url, function(data) {
		self.places.removeAll();

		for(var i = 0; i < data.length; i ++) {
			self.places.push(data[i]);
		}

		self.loading(false);
	});
};

PlacesViewModel.prototype.searchUrl = function() {
	var url =  
		'http://cpn.apphb.com/places?' +
		//'http://localhost:7844/places?' +
		(this.searchType() === 'eng' ? 'keyword=' : 'cornishKeyword=') +
		this.keyword();
	return url;
};

function PlaceViewModel() {
	this.template = ko.observable('place');

	this.EnglishName = ko.observable();
	this.Type = ko.observable();
	this.Parish = ko.observable();
	this.Keverang = ko.observable();
	this.GridReference = ko.observable();
	this.CornishName = ko.observable();
	this.Administration = ko.observable();
	this.Notes = ko.observable();
};

PlaceViewModel.prototype.load = function(id) {
	var self = this;
	$.getJSON('http://cpn.apphb.com/places/' + id, function(data) {
		self.EnglishName(data.EnglishName);
		self.Type(data.Type);
		self.Parish(data.Parish);
		self.Keverang(data.Keverang);
		self.GridReference(data.GridReference);
		self.CornishName(data.CornishName);
		self.Administration(data.Administration);
		self.Notes(data.Notes);
	});
}