import { IRemoteUser } from './../../../infrastructure/user';
import { computedFrom } from 'aurelia-binding';
import { bindable, autoinject } from "aurelia-framework";
import { ConnectionService, ConnectionStatus } from "services/connectionService";
import { State } from "store/state";
import { Store } from "aurelia-store";
import { IUser } from "infrastructure/user";

@autoinject
export class UserListItem {
    @bindable user: IRemoteUser;
    @bindable currentUser: IUser;
    
    constructor(private connectionService: ConnectionService, private store: Store<State>) {

    }

    @computedFrom('user.distanceToUser')
    get hasDistance() {
        return !(isNaN(this.user.distanceToUser));
    }
}