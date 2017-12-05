import {ITag} from './tag';

export interface IUser {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    tags: ITag[];
    location: string;
    displayName: string;
    isMentor: boolean;
    needsOnboarding: boolean;
}

export interface IUserViewModel {
    email?: string;
    firstName?: string;
    lastName?: string;
    tags?: string[];
    location?: string;
    isMentor?: boolean;
    needsOnboarding?: boolean;
}