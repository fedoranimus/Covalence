import {computedFrom} from 'aurelia-binding';
import {autoinject, bindable, customElement} from 'aurelia-framework';
import { ITag } from '../../../infrastructure/tag';
import {DOM} from 'aurelia-pal';
import { TagService } from "../../../services/tagService";

@autoinject
@customElement('tag-editor')
export class TagEditorCustomElement {
    @bindable({ attribute: "debug" }) debug = true;
    @bindable({ attribute: "existing-tags" }) tags: ITag[] = [];
    @bindable tagQuery = '';
    suggestedTags: ITag[] = [];
    errorState: string|null = null;
    private fromSelection = false;

    constructor(private element: Element, private tagService: TagService) {

    }

    onAdd() {
        if(this.tagQuery && this.tagQuery !== "") {
            const tagName = this.tagQuery;
            let event = DOM.createCustomEvent('add', { bubbles: true, cancelable: true, detail: tagName });
            this.element.dispatchEvent(event);
            this.tagQuery = "";
        }
    }
    
    @computedFrom('suggestedTags')
    get hasSuggestions() {
        return this.suggestedTags.length > 0;
    }

    async tagQueryChanged(query: string) {
        if(!this.fromSelection) {
            if(this.debug) console.log(`Query: ${query}`);
            if(query.length > 0) {
                await this.onChangeQuery(query);
            } else {
                this.suggestedTags = [];
            }
        }
        this.fromSelection = false;
    }

    async onChangeQuery(query: string) {
        if(query && !query.includes(" ")) {
            this.errorState = null;
            const potentialTags = await this.tagService.queryTag(query);
            if(potentialTags)
                this.suggestedTags = potentialTags.filter( t => this.tags.findIndex( x => x.name == t.name) === -1);
        } else {
            this.errorState = "Invalid Query; Spaces are not allowed";
        }
    }

    selectTag(tag: ITag) {
        this.fromSelection = true;
        this.tagQuery = tag.name;
        this.suggestedTags = [];
    }
}