const config = {
	variants: {
		extend: {
			backgroundColor: ['active'],
			test: ['active'],
		}
	},
	mode: "jit",
	purge: [
		"./src/**/*.{html,js,svelte,ts}",
	],
	theme: {
		extend: {
			colors: {
				'bookpage': '#E0D3AF',
			}
		},
	},
	plugins: [],
};

module.exports = config;
