<script>
    import {createEventDispatcher} from 'svelte';

    const dispatch = createEventDispatcher();
    export let show = false;
    let name = '';
    $:modalClass = show ? 'scale-100' : 'scale-0';

    function sendAndClear() {
        dispatch('create', {text: name});
        name = '';
    }
</script>
<div class="fixed top-0 left-0 w-screen h-screen flex items-center justify-center bg-gray-500 bg-opacity-50 transform {modalClass} transition-transform duration-300"
     id="modal">
    <div class="bg-purple-50 rounded-2xl" role="alert">
        <form class="w-full max-w-sm">
            <div class="flex items-center border-b border-purple-500 py-2">
                <input aria-label="Full name"
                       bind:value={name}
                       class="appearance-none bg-transparent border-none w-full text-gray-800 mr-3 py-1 px-2 leading-tight focus:outline-none"
                       placeholder="Name" type="text">
                <button class="flex-shrink-0 bg-purple-500 hover:bg-purple-700 border-purple-500 hover:border-purple-700 text-sm border-4 text-white py-1 px-2 rounded"
                        on:click={() => sendAndClear()} type="button">
                    Create
                </button>
                <button class="flex-shrink-0 border-transparent border-4 text-purple-500 hover:text-purple-800 text-sm py-1 px-2 rounded"
                        on:click={() => dispatch('cancel')} type="button">
                    Cancel
                </button>
            </div>
        </form>
    </div>
</div>