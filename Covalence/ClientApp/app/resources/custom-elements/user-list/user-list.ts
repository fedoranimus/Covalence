import { PagedList } from './../../../services/searchService';
import { computedFrom } from 'aurelia-binding';
import { ConnectionService } from './../../../services/connectionService';
import { bindable, autoinject } from "aurelia-framework";
import { IUser } from "../../../infrastructure/user";

@autoinject
export class UserList {
    @bindable users: PagedList<IUser>;

    constructor(private connectionService: ConnectionService) {

    }

    get hasResults() {
        if(!this.users) return false;
        return this.users.items.length > 0;
    }

    get hasPreviousPage() {
        if(!this.users) return false;
        return this.users.hasPreviousPage;
    }

    get hasNextPage() {
        if(!this.users) return false;
        return this.users.hasNextPage;
    }

    async nextPage() {
        console.log("TODO"); // TODO
    }

    async previousPage() {
        console.log("TODO"); // TODO
    }

    async navigateToPage(pageNumber: number) {
        console.log("TODO"); // TODO
    }

    async requestConnection(userId: string) {
        await this.connectionService.requestConnection(userId);
    }

    async confirmConnection(userId: string) {
        await this.connectionService.acceptConnection(userId);
    }

    async rejectConnection(userId: string) {
        await this.connectionService.rejectConnection(userId);
    }

    async cancelConnectionRequest(userId: string) {
        await this.connectionService.cancelConnection(userId);
    }
}

export enum connectionStatus {
    requested,
    pending,
    connected,
    blocked,
    available
}