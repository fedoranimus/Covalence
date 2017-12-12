import { bindable } from "aurelia-framework";
import { IUser } from "../../../infrastructure/user";

export class UserList {
    @bindable users: IUser[];
    constructor() {

    }

    get hasResults() {
        return this.users.length > 0;
    }
}