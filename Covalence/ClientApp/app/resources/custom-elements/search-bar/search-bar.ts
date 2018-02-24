import { bindable, autoinject } from "aurelia-framework";
import { State } from "store/state";
import { Store } from "aurelia-store";
import { addSearchParam, removeSearchParam, clearSearchParams, search } from "store/searchActions";
import { SearchService } from "services/searchService";

@autoinject
export class SearchBar {
    @bindable searchQuery;
    private state: State;

    constructor(private store: Store<State>, private searchService: SearchService) {
        store.state.subscribe(state => {
            this.state = state;
        });
    }

    bind() {
        this.store.dispatch(clearSearchParams);
    }

    updateSearchQuery(tagName: string, isAdd: boolean) {
        if(isAdd)
            this.store.dispatch(addSearchParam, tagName);
        else
            this.store.dispatch(removeSearchParam, tagName);
    }

    async search() {
        const searchQuery = this.state.searchQuery;
        this.store.dispatch(search, searchQuery, null, (searchQuery) => this.searchService.getResults(searchQuery, null));
    }
}