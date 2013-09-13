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
};

PlacesViewModel.prototype.doSearch = function() {
	console.log('Searching ...');
	var self = this;

	if(!this.keyword() || this.keyword().length < 3) {
		this.showError(true);
		return;
	}

	this.showError(false);

	$.getJSON('http://cpn.apphb.com/places?keyword=' + this.keyword(), function(data) {
		self.places.removeAll();

		for(var i = 0; i < data.length; i ++) {
			self.places.push(data[i]);
		}
	});
};

function Place(place) {
	this.name = ko.observable(place.name || '');
	this.parish = ko.observable(place.parish || '');
	this.gridRef = ko.observable(place.gridRef || '');
	this.cornishForm = ko.observable(place.cornishForm || '');
	this.progress = ko.observable(place.progress || '');
	this.notes = ko.observable(place.notes || '');
};
