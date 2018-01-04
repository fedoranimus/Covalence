import { PagedList, SearchService } from 'services/searchService';
import { computedFrom } from 'aurelia-binding';
import { ConnectionService } from 'services/connectionService';
import { bindable, autoinject } from "aurelia-framework";
import { IUser } from "infrastructure/user";

@autoinject
export class UserList {
    users: PagedList<IUser>;

    constructor(private connectionService: ConnectionService, private searchService: SearchService) {

    }

    async bind() {
        try {
            this.users = await this.searchService.getAllUsers();
            console.log('UserList', this.users);
        } catch (e) {
            console.error(e);
        }
    }

    @computedFrom('users.items.length')
    get hasResults() {
        if(!this.users) return false;
        return this.users.items.length > 0;
    }

    @computedFrom('users.hasPreviousPage')
    get hasPreviousPage() {
        if(!this.users) return false;
        return this.users.hasPreviousPage;
    }

    @computedFrom('users.hasNextPage')
    get hasNextPage() {
        if(!this.users) return false;
        return this.users.hasNextPage;
    }

    @computedFrom('users.totalPages')
    get middlePage() {
        return Math.ceil(this.users.totalPages / 2);
    }

    @computedFrom('users.totalPages')
    get middlePageMinusOne() {
        return Math.ceil(this.users.totalPages / 2) - 1;
    }

    @computedFrom('users.totalPages')
    get middlePagePlusOne() {
        return Math.ceil(this.users.totalPages / 2) + 1;
    }

    async nextPage() {
        try {
            const nextPage = ++this.users.pageNumber;
            this.users = await this.searchService.getAllUsers(nextPage);
        } catch (e) {
            console.error(e);
        }
    }

    async previousPage() {
        try {
            const previousPage = --this.users.pageNumber;
            this.users = await this.searchService.getAllUsers(previousPage);
        } catch(e) {
            console.error(e);
        }
        
    }

    async navigateToPage(pageNumber: number) {
        try {
            this.users = await this.searchService.getAllUsers(pageNumber);
        } catch(e) {
            console.error(e);
        }
    }

    async requestConnection(userId: string) {
        try {
            await this.connectionService.requestConnection(userId);
        } catch(e) {
            console.error(e);
        }
        
    }

    async confirmConnection(userId: string) {
        try {
            await this.connectionService.acceptConnection(userId);
        } catch(e) {
            console.error(e);
        }
    }

    async rejectConnection(userId: string) {
        try {
            await this.connectionService.rejectConnection(userId);
        } catch(e) {
            console.error(e);
        }
    }

    async cancelConnectionRequest(userId: string) {
        try {
            await this.connectionService.cancelConnection(userId);    
        } catch(e) {
            console.error(e);
        }
    }
}

export enum connectionStatus {
    requested,
    pending,
    connected,
    blocked,
    available
}