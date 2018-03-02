import { ITag } from './tag';
import { ConnectionStatus } from 'services/connectionService';

export interface IUser {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    tags: string[];
    bio: string;
    location: Location;
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
    distanceToUser: number;
}

export interface IUserViewModel {
    email?: string;
    firstName?: string;
    lastName?: string;
    tags?: string[];
    zipCode?: string;
    latitude?: number;
    longitude?: number;
    isMentor?: boolean;
    needsOnboarding?: boolean;
}

export interface Location {
    latitude: number;
    longitude: number;
}