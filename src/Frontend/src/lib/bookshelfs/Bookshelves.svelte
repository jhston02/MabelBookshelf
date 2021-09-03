<script>
    import Preview from "$lib/bookshelfs/Preview.svelte";
    import {flip} from 'svelte/animate';
    import {quintOut} from 'svelte/easing';
    import {fade} from 'svelte/transition';
    import DeleteBookshelfModal from "$lib/bookshelfs/DeleteBookshelfModal.svelte";
    import {getGuid} from '$lib/common/util'
    import InputNameModal from "$lib/bookshelfs/InputNameModal.svelte";

    let deleteModal = false;
    let nameModal = false;
    let colorTable = ['red', 'blue', 'green', 'pink', 'yellow', 'purple'];

    function getColor(name) {
        var hash = 0, i, chr;
        if (name.length === 0) return hash;
        for (i = 0; i < name.length; i++) {
            chr = name.charCodeAt(i);
            hash = ((hash << 5) - hash) + chr;
            hash |= 0; // Convert to 32bit integer
        }
        hash = Math.abs(hash);
        return colorTable[hash % colorTable.length];
    }

    //TODO: Source this from backend
    //TODO: Scroll to bottom on add
    let bookshelves = [
        {
            id: "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d",
            name: "Science fiction is the future",
            categories: ['scifi', 'fantasy'],
            bookIds: ['_oG_iTxP1pIC', 'F1wgqlNi8AMC', 'PDTD2hPNcjAC']
        },
        {
            id: "2b9d6bcd-bbfd-4b2d-9b5d-ab8dfbbd4bed",
            name: "Fantasy for life",
            categories: ['fantasy'],
            bookIds: ['DCxmDwAAQBAJ', '6UvVAAAAQBAJ']
        },
        {
            id: "1b9d6bcd-bbfd-4b2d-9b5d-ab8dfbbd4bed",
            name: "Biography yuck",
            categories: ['biography', 'historical'],
            bookIds: ['G40bdnyG1MYC', 'F1wgqlNi8AMC', 'byToDQAAQBAJ', 'Gm10CwAAQBAJ', 'p9UiDQAAQBAJ']
        }
    ]

    let selectedIndex = 0;

    //TODO: call out to api
    function deleteBookshelf() {
        bookshelves.splice(selectedIndex, 1);
        bookshelves = bookshelves;
        deleteModal = false;
    }

    function showModalAndSelectIndex(index) {
        selectedIndex = index;
        deleteModal = true;
    }

    function addNewBookshelf(x) {
        let name = x.detail.text;
        if (name !== '') {
            let id = getGuid();
            bookshelves.push(
                {
                    id: id,
                    name: name,
                    categories: [],
                    bookIds: []
                });
            bookshelves = bookshelves;
        }
        nameModal = false;
    }
</script>


<div class="min-h-screen bg-bookpage p-4 w-full h-full">
    <div class="md:flex md:justify-start w-full md:flex-wrap">
        {#each bookshelves as bookshelf, index (bookshelf.id)}
            <div class="p-5" transition:fade={{duration: 250, easing: quintOut}}
                 animate:flip="{{delay: 250, duration: 250, easing: quintOut}}">
                <Preview name={bookshelf.name} color={getColor(bookshelf.id)} categories="{bookshelf.categories}"
                         bookIds="{bookshelf.bookIds}" on:delete={() => showModalAndSelectIndex(index)}/>
            </div>
        {/each}
    </div>
</div>
<div class="fixed bottom-4 right-4">
    <button class="inline-flex items-center justify-center md:h-20 md:w-20 h-12 w-12 mr-2 text-indigo-100 transition-colors duration-150 bg-indigo-700 rounded-lg focus:shadow-outline hover:bg-indigo-800"
            on:click="{() => nameModal = true}">
        <svg class="md:h-16 md:w-16 h-8 w-8 fill-current" viewBox="0 0 20 20">
            <path clip-rule="evenodd"
                  d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z"
                  fill-rule="evenodd"></path>
        </svg>
    </button>
</div>
<DeleteBookshelfModal bind:show={deleteModal} on:cancel={() => deleteModal = false}
                      on:continue={() => deleteBookshelf()}/>
<InputNameModal bind:show={nameModal} on:cancel={() => nameModal = false} on:create={addNewBookshelf}/>