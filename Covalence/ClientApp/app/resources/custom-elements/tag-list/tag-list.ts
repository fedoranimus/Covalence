import {autoinject, bindable} from 'aurelia-framework';

@autoinject
export class TagList {
    public canEdit = false;
    @bindable tags: any[] = [];
    constructor() {

    }

    public toggleEdit() {
        this.canEdit = !this.canEdit;
    }

    public removeTag(tag) {
        const index = this.tags.findIndex(x => x.name == tag.name);
        this.tags.splice(index, 1);
    }

    public addTag(tag) {
        const index = this.tags.findIndex(x => x.name == tag.name);
        if(index === -1)
            this.tags.push(tag);
    }
}