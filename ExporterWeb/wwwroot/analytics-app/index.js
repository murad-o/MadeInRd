const origin = window.parent.document.getElementById('origin-api').value;

async function fetchCustomsRecords(signal, params) {
	const url = `${origin}/?${new URLSearchParams(params)}`;
	const resp = await fetch(url, { signal, cache: 'force-cache' });
	return resp.json();
}

async function fetchRegions() {
	const resp = await fetch('regions.json');
	return resp.json();
}

async function fetchCountries() {
	const resp = await fetch('countries.json');
	return resp.json();
}

const main = {
	data() {
		return {
			customsRecordsAbortController: new AbortController(),
			data: [],
			loaded: false,
			maxRecordsToShow: 10,
			allRegions: {},
			allCountries: {},

			dateFrom: '2017-01-01',
			dateTo: null,
			direction: '',
			tnved: '',
			regions: [],
			// Not implemented yet
			countries: [],
		}
	},
	methods: {
		async updateCustomsRecords() {
			this.loaded = false;
			const params = {};
			if (this.dateFrom)
				params.dateFrom = this.toDate(this.dateFrom);
			if (this.dateTo)
				params.dateTo = this.toDate(this.dateTo);
			if (this.direction)
				params.direction = this.direction;
			if (this.tnved)
				params.tnved = this.tnved;
			if (this.regions.length !== 0)
				params.regions = this.regions.join(',');
			if (this.countries.length !== 0)
				params.countries = this.countries.join(',');

			this.customsRecordsAbortController.abort();
			this.customsRecordsAbortController = new AbortController();
			this.data = await fetchCustomsRecords(this.customsRecordsAbortController.signal, params);
			this.loaded = true;
		},
		toDate(dateAsString) {
			const date = new Date(dateAsString);
			const year = date.getFullYear();
			const month = (date.getMonth() + 1).toString().padStart(2, '0');
			return `${year}${month}`;
		},
		numberOfRecordsToShow(records) {
			const nums = Number(this.maxRecordsToShow);
			if (nums >= records.length)
				return records.length.toString();
			return `${nums} / ${records.length}`;
		}
	},
	async mounted() {
		this.updateCustomsRecords();
		this.allRegions = await fetchRegions();
		this.allCountries = await fetchCountries();
	}
}

const app = Vue.createApp(main).mount('#statistics-app');
