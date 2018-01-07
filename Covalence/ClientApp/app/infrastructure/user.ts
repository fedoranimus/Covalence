import { ITag } from './tag';
import { ConnectionStatus } from 'services/connectionService';

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
    emailConfirmed: boolean;
}

export interface IRemoteUser {
    id: string;
    email?: string;
    firstName: string;
    lastName: string;
    bio: string;
    tags: ITag[];
    connectionStatus: ConnectionStatus;
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