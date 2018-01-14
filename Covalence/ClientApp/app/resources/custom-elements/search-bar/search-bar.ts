import { bindable, autoinject } from "aurelia-framework";
import { State } from "store/state";
import { Store } from "aurelia-store";
import { addSearchParam, removeSearchParam, clearSearchParams } from "store/searchActions";

@autoinject
export class SearchBar {
    @bindable searchQuery;
    constructor(private store: Store<State>) {

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
}