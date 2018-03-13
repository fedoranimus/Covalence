import { PagedList, SearchService } from 'services/searchService';
import { computedFrom } from 'aurelia-binding';
import { bindable, autoinject } from "aurelia-framework";
import { IUser, IRemoteUser } from "infrastructure/user";
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { navigateToPage, search, clearSearchParams } from 'store/searchActions';

@autoinject
export class UserList {
    @bindable results: PagedList<IRemoteUser>;
    @bindable searchQuery: string[];
    @bindable currentUser: IUser;

    constructor(private searchService: SearchService, private store: Store<State>) {

    }

    created() {
        const searchQuery = [];
        this.store.dispatch(clearSearchParams);
        this.store.dispatch(search, searchQuery, null, (searchQuery) => this.searchService.getResults(searchQuery));
    }

    @computedFrom('results.items.length')
    get hasResults() {
        if(!this.results) return false;
        return this.results.items.length > 0;
    }

    @computedFrom('results.hasPreviousPage')
    get hasPreviousPage() {
        if(!this.results) return false;
        return this.results.hasPreviousPage;
    }

    @computedFrom('results.hasNextPage')
    get hasNextPage() {
        if(!this.results) return false;
        return this.results.hasNextPage;
    }

    @computedFrom('results.totalPages')
    get middlePage() {
        return Math.ceil(this.results.totalPages / 2);
    }

    @computedFrom('results.totalPages')
    get middlePageMinusOne() {
        return Math.ceil(this.results.totalPages / 2) - 1;
    }

    @computedFrom('results.totalPages')
    get middlePagePlusOne() {
        return Math.ceil(this.results.totalPages / 2) + 1;
    }

    nextPage() {
        const nextPage = ++this.results.pageNumber;
        const searchQuery = this.searchQuery;
        this.store.dispatch(search, searchQuery, nextPage, (searchQuery, nextPage) => this.searchService.getResults(searchQuery, nextPage));
    }

    previousPage() {
        const previousPage = --this.results.pageNumber;
        const searchQuery = this.searchQuery;
        this.store.dispatch(search, searchQuery, previousPage, (searchQuery, previousPage) => this.searchService.getResults(searchQuery, previousPage));
    }

    navigateToPage(pageNumber: number) {
        const searchQuery = this.searchQuery;
        this.store.dispatch(search, searchQuery, pageNumber, (searchQuery, pageNumber) => this.searchService.getResults(searchQuery, pageNumber));
    }
}