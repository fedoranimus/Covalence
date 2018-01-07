import { PagedList, SearchService } from 'services/searchService';
import { computedFrom } from 'aurelia-binding';
import { ConnectionService, ConnectionStatus } from 'services/connectionService';
import { bindable, autoinject } from "aurelia-framework";
import { IUser } from "infrastructure/user";
import { Store } from 'aurelia-store';
import { State } from 'store/state';
import { acceptConnection, updateConnection } from 'store/connectionActions';
import { getAll, navigateToPage } from 'store/searchActions';

@autoinject
export class UserList {
    @bindable results: PagedList<IUser>;

    constructor(private connectionService: ConnectionService, private searchService: SearchService, private store: Store<State>) {

    }

    created() {
        this.store.dispatch(getAll, () => this.searchService.getAllUsers());
    }

    // async bind() {
    //     try {
    //         this.users = await this.searchService.getAllUsers();
    //         console.log('UserList', this.users);
    //     } catch (e) {
    //         console.error(e);
    //     }
    // }

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

    async nextPage() {
        try {
            const nextPage = ++this.results.pageNumber;
            this.store.dispatch(navigateToPage, nextPage, (nextPage) => this.searchService.getAllUsers(nextPage));
        } catch (e) {
            console.error(e);
        }
    }

    async previousPage() {
        try {
            const previousPage = --this.results.pageNumber;
            this.store.dispatch(navigateToPage, previousPage, (previousPage) => this.searchService.getAllUsers(previousPage));
        } catch(e) {
            console.error(e);
        }
        
    }

    async navigateToPage(pageNumber: number) {
        try {
            this.store.dispatch(navigateToPage, pageNumber, (pageNumber) => this.searchService.getAllUsers(pageNumber));
        } catch(e) {
            console.error(e);
        }
    }

    async requestConnection(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.requested, (userId) => this.connectionService.requestConnection(userId));
        } catch(e) {
            console.error(e);
        }
        
    }

    async confirmConnection(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.connected, (userId) => this.connectionService.acceptConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }

    async rejectConnection(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.available, (userId) => this.connectionService.rejectConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }

    async cancelConnectionRequest(userId: string) {
        try {
            this.store.dispatch(updateConnection, userId, ConnectionStatus.available, (userId) => this.connectionService.cancelConnection(userId));
        } catch(e) {
            console.error(e);
        }
    }
}