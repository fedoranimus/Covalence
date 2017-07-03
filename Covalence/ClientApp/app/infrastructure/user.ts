import {ITag} from './tag';

export interface IUser {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    tags: ITag[];
    location: string;
    displayName: string;
}

export interface IUserViewModel {
    email?: string;
    firstName?: string;
    lastName?: string;
    tags?: string[];
    location?: string;
}