async function getData() {
	return new Promise(res => setTimeout(res, 1000, {data: [1, 2, 3]}));
}

const Counter = {
	data() {
		return {
			data: [],
			loaded: false,
		}
	},
	async mounted() {
		const { data } = await getData();
		this.data = data;
		this.loaded = true;
	}
}

Vue.createApp(Counter).mount('#statistics-app');
