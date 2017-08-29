// Write your Javascript code.
var app = new Vue({
    el: '#recipe-table',
    data: {
        recipes: []
    },
    created: function() {
        this.loadRecipes();
    },
    methods: {
        loadRecipes: function () {
            var vm = this;
            axios.get('api/recipes')
                .then(function(response) {
                    vm.recipes = response.data.items;
                });
        }
    }
});