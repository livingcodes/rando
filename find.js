function find(input) {
	return (input.startsWith('#'))
			? document.getElementById(input)
		: (input.startsWith('.'))
			? document.getElementsByClassName(input)
			: document.getElementsByTagName(input)
}
post = find('#post')
comments = find('.comment')
keywords = find('li')

keywords = find('#keywords li')
keywords = find('.keywords li')
keywords = find('ul li')