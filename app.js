var rootUrl = "http://cpn.apphb.com/";

function Application() {
	var self = this;
	this.viewModelCache = {};
	this.currentViewModel = ko.observable(this.getViewModel('home', HomeViewModel));
	this.api = null;

	new HDK(rootUrl).fetch(function(response) {
		self.api = response;

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
	});
};

Application.prototype.getViewModel = function(name, constructor) {
	if (!this.viewModelCache[name]) {
		this.viewModelCache[name] = new constructor();
		this.viewModelCache[name].parent = this;
	}

	return this.viewModelCache[name];
}

Application.prototype.link = function(data, event) {
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

	this.currentPage = ko.observable();
	this.hasPrevious = ko.observable(false);
	this.hasNext = ko.observable(false);
	this.totalPages = ko.observable(0);

	this.nextUrl = ko.observable();
	this.previousUrl = ko.observable();
};

PlacesViewModel.prototype.doSearch = function() {
	var self = this;

	if (!this.keyword() || this.keyword().length < 3) {
		this.showError(true);
		return;
	}

	this.loading(true);
	this.showError(false);

	var linkName = this.searchType() == 'eng' ? 'places-english' : 'places-cornish';
	this.parent.api.follow(linkName, {
		keyword: this.keyword(),
		page: 1,
		pageSize: 10
	}, function(data) {
		self.processResults(data);
	});
};

PlacesViewModel.prototype.next = function() {
	var self = this;

	this.loading(true);
	this.showError(false);

	this.parent.api.fetch(this.nextUrl(),function(data) { self.processResults(data); });
};

PlacesViewModel.prototype.previous = function() {
	var self = this;

	this.loading(true);
	this.showError(false);

	this.parent.api.fetch(this.previousUrl(),function(data) { self.processResults(data); });
};

PlacesViewModel.prototype.processResults = function(data) {
	this.places.removeAll();

	for (var i = 0; i < data._embedded.places.length; i++) {
		this.places.push(data._embedded.places[i]);
	}

	this.hasNext(false);
	if (data._links.next) {
		this.hasNext(true);
		this.nextUrl(data._links.next.href);
	}

	this.hasPrevious(false);
	if (data._links.prev) {
		this.hasPrevious(true);
		this.previousUrl(data._links.prev.href);
	}

	this.totalPages(data.pages);
	this.currentPage(data.page);

	this.loading(false);
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
	this.Longitude = ko.observable();
	this.Latitude = ko.observable();

	this.map = null;
};

PlaceViewModel.prototype.load = function(url) {
	var self = this;

	this.parent.api.fetch(url, function(data) {
		self.EnglishName(data.EnglishName);
		self.Type(data.Type);
		self.Parish(data.Parish);
		self.Keverang(data.Keverang);
		self.GridReference(data.GridReference);
		self.CornishName(data.CornishName);
		self.Administration(data.Administration);
		self.Notes(data.Notes);
		self.Longitude(data.longitude);
		self.Latitude(data.latitude);

		self.initMap();
	});
};

PlaceViewModel.prototype.initMap = function() {
	if(this.Longitude() && this.Latitude()) {
		var coords = new google.maps.LatLng(this.Latitude(),this.Longitude());
		var mapOptions = {
			zoom: 12,
			center: coords
		};

		this.map = new google.maps.Map(document.getElementById('google-map'), mapOptions);

		var marker = new google.maps.Marker({
      		position: coords,
      		map: this.map,
      		title: this.CornishName()
  		});
	}
};

/*
var map;
function initialize() {
  var mapOptions = {
    zoom: 8,
    center: new google.maps.LatLng(-34.397, 150.644)
  };
  map = new google.maps.Map(document.getElementById('map-canvas'),
      mapOptions);
}
*/