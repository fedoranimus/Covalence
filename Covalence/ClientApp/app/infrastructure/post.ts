import {IUser} from './user';
import {ITag} from './tag';

export interface IPost {
    postId: number;
    title: string;
    content: string;
    author: IUser;
    category: number; //TODO: Make an enumeration
    dateCreated:string;
    dateModified:string;
    tags: ITag[];
}