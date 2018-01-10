export class FilterValueConverter {
    toView(array, searchTerm, filterFunc) {
        return array.filter(item => {
            const matches = searchTerm !== null ? filterFunc(searchTerm, item): true;

            return matches;
        });
    }
}