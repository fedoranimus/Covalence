import { autoinject, bindable } from 'aurelia-framework';
import { TagService } from '../../../services/tagService';
import { ITag } from '../../../infrastructure/tag';

@autoinject
export class TagList {
    public canEdit = false;
    @bindable tags: ITag[] = [];
    constructor(private tagService: TagService) {

    }

    get hasTags() {
        return this.tags.length > 0;
    }

    toggleEdit() {
        if(this.canEdit) {
            //TODO: Save the user with this list of tags
            console.log(`TODO: Saving User`);
        }

        this.canEdit = !this.canEdit;
    }

    onRemoveTag(tag) {
        const index = this.tags.findIndex(x => x.name == tag.name);
        this.tags.splice(index, 1);
    }

    async onAddTag(event: CustomEvent) {
        const tagName = event.detail;
        const index = this.tags.findIndex(x => x.name == tagName);
        const tag = await this.tagService.getTag(tagName);
        if(index === -1)
            this.tags.push(tag);
    }
}